using EnvironmentVarsApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EnvironmentVarsApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с конфигурацией
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="configuration"></param>
    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetEnvironmentVariableNamesAsync()
    {
        var names = _configuration.GetSection("EnvironmentVariables").Get<List<string>>();
        return names ?? new List<string>();
    }
}
