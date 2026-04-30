using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Services;
using EnvironmentVarsApp.Infrastructure.Logging;
using EnvironmentVarsApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EnvironmentVarsApp.Extensions;

/// <summary>
/// Расширения для настройки DI контейнера
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Настроить сервисы приложения
    /// </summary>
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // Logging
        services.AddSingleton<ILoggingService, LoggingService>();

        // Environment Variables
        services.AddSingleton<IEnvironmentVariableService, EnvironmentVariableService>();

        // Manager Service
        services.AddSingleton<IEnvironmentVariableManagerService, EnvironmentVariableManagerService>();

        return services;
    }

    /// <summary>
    /// Создать конфигурацию из appsettings.json
    /// </summary>
    public static IConfiguration CreateConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        return builder.Build();
    }
}
