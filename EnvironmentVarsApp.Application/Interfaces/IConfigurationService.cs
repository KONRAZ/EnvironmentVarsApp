namespace EnvironmentVarsApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с конфигурацией
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Получить имена переменных из конфигурации
    /// </summary>
    Task<IEnumerable<string>> GetEnvironmentVariableNamesAsync();
}
