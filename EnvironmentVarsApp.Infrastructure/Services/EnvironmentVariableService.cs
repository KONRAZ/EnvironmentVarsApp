using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с переменными среды
/// </summary>
public class EnvironmentVariableService : IEnvironmentVariableService
{
    private readonly ILoggingService _loggingService;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="loggingService"></param>
    public EnvironmentVariableService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EnvironmentVariable>> GetEnvironmentVariablesAsync(IEnumerable<string> variableNames)
    {
        var variables = new List<EnvironmentVariable>();

        foreach (var name in variableNames)
        {
            var variable = await GetEnvironmentVariableAsync(name);
            if (variable != null)
            {
                variables.Add(variable);
            }
        }

        return variables;
    }

    /// <inheritdoc/>
    public async Task<EnvironmentVariable?> GetEnvironmentVariableAsync(string name)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        
        var variable = new EnvironmentVariable(name, value ?? string.Empty);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Read", name, null, value);
        
        return variable;
    }

    /// <inheritdoc/>
    public async Task SetEnvironmentVariableAsync(EnvironmentVariable variable)
    {
        var oldValue = Environment.GetEnvironmentVariable(variable.Name, EnvironmentVariableTarget.Machine);
        
        Environment.SetEnvironmentVariable(variable.Name, variable.Value, EnvironmentVariableTarget.Machine);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Write", variable.Name, oldValue, variable.Value);
    }

    /// <inheritdoc/>
    public async Task DeleteEnvironmentVariableAsync(string name)
    {
        var oldValue = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        
        Environment.SetEnvironmentVariable(name, null, EnvironmentVariableTarget.Machine);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Delete", name, oldValue, null);
    }
}
