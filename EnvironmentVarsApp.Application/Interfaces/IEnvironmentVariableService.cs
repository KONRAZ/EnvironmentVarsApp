using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с переменными среды
/// </summary>
public interface IEnvironmentVariableService
{
    /// <summary>
    /// Получить все переменные
    /// </summary>
    Task<IEnumerable<EnvironmentVariable>> GetEnvironmentVariablesAsync(IEnumerable<string> variableNames);

    /// <summary>
    /// Получить переменную по имени
    /// </summary>
    Task<EnvironmentVariable?> GetEnvironmentVariableAsync(string name);

    /// <summary>
    /// Установить значение переменной
    /// </summary>
    Task SetEnvironmentVariableAsync(EnvironmentVariable variable);
}
