using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Infrastructure.Email;
using MoneyMarket.Infrastructure.Files;
using MoneyMarket.Infrastructure.Identity;
using MoneyMarket.Infrastructure.Ids;
using MoneyMarket.Infrastructure.Notifications;

namespace MoneyMarket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDateTime, SystemDateTime>();
        services.AddSingleton<IGuidGenerator, GuidGenerator>();

        if (config.GetValue<bool>("UseAzureBlob"))
        {
            services.AddSingleton(new BlobServiceClient(config["AzureBlob:ConnectionString"]));
            services.AddScoped<IFileStorage, AzureBlobStorage>();
        }
        else
        {
            services.AddScoped<IFileStorage, LocalFileStorage>();
        }
        //services.AddScoped<IEmailSender, SmtpEmailSender /* or SendGridEmailSender */>();
        services.AddScoped<IEmailSender, ConsoleEmailSender /* or SendGridEmailSender */>();

        services.AddSingleton<INotificationService, NoOpNotificationService>();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}

public sealed class SystemDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}

