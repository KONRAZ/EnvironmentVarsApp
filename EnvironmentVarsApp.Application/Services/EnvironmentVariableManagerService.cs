using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Application.Services;

/// <summary>
/// Реализация сервиса для управления переменными среды
/// </summary>
public class EnvironmentVariableManagerService : IEnvironmentVariableManagerService
{
    private readonly IEnvironmentVariableService _environmentVariableService;
    private readonly IConfigurationService _configurationService;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="environmentVariableService"></param>
    /// <param name="configurationService"></param>
    public EnvironmentVariableManagerService(
        IEnvironmentVariableService environmentVariableService,
        IConfigurationService configurationService)
    {
        _environmentVariableService = environmentVariableService;
        _configurationService = configurationService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EnvironmentVariable>> LoadVariablesAsync()
    {
        var variableNames = await _configurationService.GetEnvironmentVariableNamesAsync();
        return await _environmentVariableService.GetEnvironmentVariablesAsync(variableNames);
    }

    /// <inheritdoc/>
    public async Task SaveVariableAsync(EnvironmentVariable variable)
    {
        await _environmentVariableService.SetEnvironmentVariableAsync(variable);
    }

    /// <inheritdoc/>
    public async Task DeleteVariableAsync(string name)
    {
        await _environmentVariableService.DeleteEnvironmentVariableAsync(name);
    }
}
