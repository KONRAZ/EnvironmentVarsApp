using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с переменными среды
/// </summary>
public class EnvironmentVariableService : IEnvironmentVariableService
{
    private readonly ILoggingService _loggingService;
    private readonly ICommentService _commentService;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="loggingService"></param>
    /// <param name="commentService"></param>
    public EnvironmentVariableService(ILoggingService loggingService, ICommentService commentService)
    {
        _loggingService = loggingService;
        _commentService = commentService;
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
        var comment = await _commentService.GetCommentAsync(name);
        
        var variable = new EnvironmentVariable(name, value ?? string.Empty, comment);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Read", name, null, value);
        
        return variable;
    }

    /// <inheritdoc/>
    public async Task SetEnvironmentVariableAsync(EnvironmentVariable variable)
    {
        if (string.IsNullOrWhiteSpace(variable.Value))
        {
            return; // Не сохраняем переменные без значений
        }

        var oldValue = Environment.GetEnvironmentVariable(variable.Name, EnvironmentVariableTarget.Machine);
        
        Environment.SetEnvironmentVariable(variable.Name, variable.Value, EnvironmentVariableTarget.Machine);
        
        await _commentService.SetCommentAsync(variable.Name, variable.Comment);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Write", variable.Name, oldValue, variable.Value);
    }

    /// <inheritdoc/>
    public async Task DeleteEnvironmentVariableAsync(string name)
    {
        var oldValue = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        
        Environment.SetEnvironmentVariable(name, null, EnvironmentVariableTarget.Machine);
        
        await _commentService.DeleteCommentAsync(name);
        
        await _loggingService.LogEnvironmentVariableOperationAsync("Delete", name, oldValue, null);
    }
}
