using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyMarket.Persistence.Context;
using MoneyMarket.Persistence.Identity;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Connection string
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// 🔹 DbContexts (two separate ones: Identity + Domain)
builder.Services.AddDbContext<IdentityDbContextMM>(o => o.UseSqlServer(conn));
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));

// 🔹 ASP.NET Core Identity
builder.Services
    .AddIdentityCore<ApplicationUser>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.User.RequireUniqueEmail = true;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContextMM>()
    .AddSignInManager();

// 🔹 JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 🔹 Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanApproveLoan", p => p.RequireRole("Admin", "CRM"));
    options.AddPolicy("CanDisburseLoan", p => p.RequireRole("Admin"));
    options.AddPolicy("CanFundLoan", p => p.RequireRole("Lender"));
});

// 🔹 Logging (Serilog)
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// 🔹 MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// 🔹 ProblemDetails (must be registered BEFORE app.Build)
builder.Services.AddProblemDetails(options =>
{
    options.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
    options.MapToStatusCode<InvalidOperationException>(StatusCodes.Status409Conflict);
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

// 🔹 Controllers + FluentValidation
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware pipeline
app.UseProblemDetails();   // should come very early
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 🔹 Ensure databases are created/migrated & seed Admin user
using (var scope = app.Services.CreateScope())
{
    var idCtx = scope.ServiceProvider.GetRequiredService<IdentityDbContextMM>();
    await idCtx.Database.MigrateAsync();

    var appCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await appCtx.Database.MigrateAsync();

    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();
app.Run();
