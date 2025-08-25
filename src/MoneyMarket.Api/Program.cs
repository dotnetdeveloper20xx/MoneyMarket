using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyMarket.Api.Common.Services;
using MoneyMarket.Application.Common.Abstractions;     // IAppDbContext, IJwtTokenService, IIdentityService
using MoneyMarket.Application.Common.Behaviors;       // ValidationBehavior
using MoneyMarket.Application.Common.Models;          // ApiResponse<>

using MoneyMarket.Infrastructure.Identity;            // IdentityService
using MoneyMarket.Persistence.Context;                // AppDbContext
using MoneyMarket.Persistence.Identity;               // IdentityDbContextMM
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────
// Connection string
// ─────────────────────────────────────────────
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// ─────────────────────────────────────────────
// DbContexts
//   • Identity DB (for ASP.NET Identity)
//   • App DB (for domain data) + map IAppDbContext
// ─────────────────────────────────────────────
builder.Services.AddDbContext<IdentityDbContextMM>(o => o.UseSqlServer(conn));

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));
builder.Services.AddScoped<IAppDbContext>(sp => (IAppDbContext)sp.GetRequiredService<AppDbContext>());

// ─────────────────────────────────────────────
// ASP.NET Core Identity (single registration!)
// ─────────────────────────────────────────────
builder.Services
    .AddIdentityCore<ApplicationUser>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.User.RequireUniqueEmail = true;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContextMM>()
    .AddSignInManager();

// ─────────────────────────────────────────────
// JWT Authentication
// ─────────────────────────────────────────────
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"] ?? issuer;
        var key = builder.Configuration["Jwt:Key"]!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30), // small dev leeway

            // critical for slim controllers
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });


// ─────────────────────────────────────────────
// Authorization policies
// ─────────────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanApproveLoan", p => p.RequireRole("Admin", "CRM"));
    options.AddPolicy("CanDisburseLoan", p => p.RequireRole("Admin"));
    options.AddPolicy("CanFundLoan", p => p.RequireRole("Lender"));
});

// ─────────────────────────────────────────────
// Logging (Serilog)
// ─────────────────────────────────────────────
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// ─────────────────────────────────────────────
// MediatR + FluentValidation (Application assembly)
// ─────────────────────────────────────────────
var appAssembly = typeof(ApiResponse<>).Assembly; // MoneyMarket.Application
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
builder.Services.AddValidatorsFromAssembly(appAssembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Infrastructure services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

// Access HttpContext in services
builder.Services.AddHttpContextAccessor();

// Current user abstraction
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ─────────────────────────────────────────────
// ProblemDetails
// ─────────────────────────────────────────────
builder.Services.AddProblemDetails(options =>
{
    options.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
    options.MapToStatusCode<InvalidOperationException>(StatusCodes.Status409Conflict);
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);

    options.Map<ValidationException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Validation failed",
        Status = StatusCodes.Status400BadRequest,
        Detail = string.Join("; ", ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))
    });
});

// ─────────────────────────────────────────────
// Controllers & Swagger
// ─────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoneyMarket API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
});

var app = builder.Build();

// ─────────────────────────────────────────────
// Middleware pipeline
// ─────────────────────────────────────────────
app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

// ─────────────────────────────────────────────
// Ensure databases are migrated & seed Admin user
// ─────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var idCtx = scope.ServiceProvider.GetRequiredService<IdentityDbContextMM>();
    await idCtx.Database.MigrateAsync();

    var appCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await appCtx.Database.MigrateAsync();

    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.Run();
