using EnvironmentVarsApp.Application.Interfaces;
using System.Text.Json;

namespace EnvironmentVarsApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с комментариями переменных
/// </summary>
public class CommentService : ICommentService
{
    private readonly string _commentsFilePath;
    private readonly Dictionary<string, string> _comments;
    private readonly object _lock = new object();

    /// <summary>
    /// .ctor
    /// </summary>
    public CommentService()
    {
        _commentsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comments.json");
        _comments = new Dictionary<string, string>();
        LoadComments();
    }
    
    /// <inheritdoc/>
    public async Task<string> GetCommentAsync(string variableName)
    {
        await Task.CompletedTask;
        
        lock (_lock)
        {
            return _comments.TryGetValue(variableName, out var comment) ? comment : string.Empty;
        }
    }

    /// <inheritdoc/>
    public async Task SetCommentAsync(string variableName, string comment)
    {
        await Task.CompletedTask;
        
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                _comments.Remove(variableName);
            }
            else
            {
                _comments[variableName] = comment;
            }
            
            SaveComments();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteCommentAsync(string variableName)
    {
        await Task.CompletedTask;
        
        lock (_lock)
        {
            _comments.Remove(variableName);
            SaveComments();
        }
    }
    
    private void LoadComments()
    {
        try
        {
            if (File.Exists(_commentsFilePath))
            {
                var json = File.ReadAllText(_commentsFilePath);
                var loadedComments = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (loadedComments != null)
                {
                    foreach (var kvp in loadedComments)
                    {
                        _comments[kvp.Key] = kvp.Value;
                    }
                }
            }
        }
        catch (Exception)
        {
            // Если файл не удалось загрузить, начинаем с пустым словарем
        }
    }

    private void SaveComments()
    {
        try
        {
            var json = JsonSerializer.Serialize(_comments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_commentsFilePath, json);
        }
        catch (Exception)
        {
            // Логируем ошибку, но не прерываем работу
        }
    }

    
}
