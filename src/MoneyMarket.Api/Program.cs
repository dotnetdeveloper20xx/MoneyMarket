using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Persistence.Context;
using MoneyMarket.Persistence.Seed;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR + FluentValidation
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllers();

// Add FluentValidation auto-validation and client-side adapters as recommended
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Register validators from assembly containing Program
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//// ProblemDetails middleware
//builder.Services.AddProblemDetails();

//// OpenTelemetry
//builder.Services.AddOpenTelemetry()
//.WithTracing(t => t
//.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FaziMoneyMarket.Api"))
//.AddAspNetCoreInstrumentation()
//.AddHttpClientInstrumentation()
//.AddEntityFrameworkCoreInstrumentation()
//.AddOtlpExporter());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed database
await AppDbContextSeed.SeedAsync(app.Services);

app.Run();
