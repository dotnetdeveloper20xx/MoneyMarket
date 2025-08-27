using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyMarket.Api.Common.Services;              // CurrentUserService
// Existing namespaces
using MoneyMarket.Application;                       // AddApplication()
using MoneyMarket.Application.Common.Abstractions;  // ICurrentUserService
using MoneyMarket.Application.Common.Behaviors;     // (for types if needed in compile)
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Common.Models;        // ApiResponse<>
using MoneyMarket.Infrastructure;                   // AddInfrastructure()
using MoneyMarket.Infrastructure.Identity;          // IdentityService
using MoneyMarket.Persistence;                      // AddPersistence()
using MoneyMarket.Persistence.Identity;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────
// Logging (Serilog)
// ─────────────────────────────────────────────
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// ─────────────────────────────────────────────
// Authentication (JWT)
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
            ClockSkew = TimeSpan.FromSeconds(30),
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
// Clean DI wiring by project
// ─────────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Infrastructure services you already have (JwtTokenService/IdentityService)
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddApplication();                       // MediatR, Validators, Behaviors
builder.Services.AddInfrastructure(builder.Configuration); // DateTime/Guid/etc.
builder.Services.AddPersistence(builder.Configuration);    // DbContexts, Repositories, UoW


// ─────────────────────────────────────────────
// ProblemDetails middleware
// ─────────────────────────────────────────────
builder.Services.AddProblemDetails(options =>
{
    // Map specific known exceptions to HTTP status codes
    options.Map<NotFoundException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Not Found",
        Status = StatusCodes.Status404NotFound,
        Detail = ex.Message
    });

    options.Map<ConflictException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Conflict",
        Status = StatusCodes.Status409Conflict,
        Detail = ex.Message
    });

    options.Map<ForbiddenAccessException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Forbidden",
        Status = StatusCodes.Status403Forbidden,
        Detail = ex.Message
    });

    // Auth-related
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);

    // Argument-level problems → 400
    options.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
    options.MapToStatusCode<ArgumentException>(StatusCodes.Status400BadRequest);

    // FluentValidation errors → 400, include messages
    options.Map<FluentValidation.ValidationException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Validation failed",
        Status = StatusCodes.Status400BadRequest,
        Detail = string.Join("; ", ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))
    });

    // Optional: your own Application.ValidationException (if you’ve created it)
    options.Map<MoneyMarket.Application.Common.Exceptions.ValidationException>(ex =>
        new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Detail = ex.Message
        });

    // Last resort — anything else → 500
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
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
// Migrate & seed
// ─────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    await PersistenceMigrationRunner.RunAsync(scope.ServiceProvider);
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.Run();
