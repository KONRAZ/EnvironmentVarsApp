using EnvironmentVarsApp.Application.Interfaces;
using Serilog;

namespace EnvironmentVarsApp.Infrastructure.Logging;

/// <summary>
/// Реализация сервиса для логирования
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly ILogger _logger;

    public LoggingService()
    {
        _logger = new LoggerConfiguration()
            .WriteTo.File($"test-sms-wpf-app-{DateTime.Now:yyyyMMdd}.log")
            .CreateLogger();
    }

    public async Task LogEnvironmentVariableOperationAsync(string operation, string variableName, string? oldValue = null, string? newValue = null)
    {
        var message = oldValue != null 
            ? $"Operation: {operation} | Variable: {variableName} | OldValue: {oldValue} | NewValue: {newValue}"
            : $"Operation: {operation} | Variable: {variableName} | Value: {newValue}";

        _logger.Information(message);
        
        await Task.CompletedTask;
    }
}
