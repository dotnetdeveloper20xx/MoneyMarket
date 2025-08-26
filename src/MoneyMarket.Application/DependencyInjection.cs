using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoneyMarket.Application.Common.Behaviors;
using AutoMapper;

namespace MoneyMarket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Safer than GetExecutingAssembly when invoked from different loaders (e.g., tests)
        var appAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(appAssembly);

            // Pipeline order: outer → inner
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

        // FluentValidation – scan validators in Application
        services.AddValidatorsFromAssembly(appAssembly);

        services.AddSingleton<IMapper>(sp =>
        {
            var cfg = new MapperConfiguration(mc => mc.AddMaps(typeof(DependencyInjection).Assembly));
            cfg.AssertConfigurationIsValid();
            // For AutoMapper versions that expect Func<Type, object>
            return cfg.CreateMapper(type => sp.GetRequiredService(type));
        });

        return services;
    }
}
