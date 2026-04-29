namespace EnvironmentVarsApp.Application.Interfaces;

/// <summary>
/// Сервис для логирования
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// Записать операцию с переменной среды
    /// </summary>
    Task LogEnvironmentVariableOperationAsync(string operation, string variableName, string? oldValue = null, string? newValue = null);
}
