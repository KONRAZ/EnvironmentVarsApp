namespace EnvironmentVarsApp.Application.Interfaces;

/// <summary>
/// Сервис для работы с комментариями переменных
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Получить комментарий для переменной
    /// </summary>
    Task<string> GetCommentAsync(string variableName);

    /// <summary>
    /// Сохранить комментарий для переменной
    /// </summary>
    Task SetCommentAsync(string variableName, string comment);

    /// <summary>
    /// Удалить комментарий для переменной
    /// </summary>
    Task DeleteCommentAsync(string variableName);
}
