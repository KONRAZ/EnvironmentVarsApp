using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Application.Interfaces;

/// <summary>
/// Сервис для управления переменными среды
/// </summary>
public interface IEnvironmentVariableManagerService
{
    /// <summary>
    /// Загрузить все переменные
    /// </summary>
    Task<IEnumerable<EnvironmentVariable>> LoadVariablesAsync();

    /// <summary>
    /// Сохранить переменную
    /// </summary>
    Task SaveVariableAsync(EnvironmentVariable variable);

    /// <summary>
    /// Удалить переменную
    /// </summary>
    Task DeleteVariableAsync(string name);
}
