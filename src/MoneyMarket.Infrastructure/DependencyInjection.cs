using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDateTime, SystemDateTime>();
        services.AddSingleton<IGuidGenerator, GuidGenerator>();
       
        return services;
    }
}

public sealed class SystemDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public interface IGuidGenerator
{
    Guid NewGuid();
}

public sealed class GuidGenerator : IGuidGenerator
{
    public Guid NewGuid() => Guid.NewGuid();
}
