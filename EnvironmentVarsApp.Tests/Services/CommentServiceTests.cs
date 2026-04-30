using EnvironmentVarsApp.Infrastructure.Services;
using System.Text.Json;

namespace EnvironmentVarsApp.Tests.Services;

/// <summary>
/// Тесты для CommentService
/// </summary>
public class CommentServiceTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testCommentsFile;
    private readonly CommentService _commentService;

    public CommentServiceTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _testCommentsFile = Path.Combine(_testDirectory, "comments.json");
        
        _commentService = new CommentService();
        
        var fieldInfo = typeof(CommentService).GetField("_commentsFilePath", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo?.SetValue(_commentService, _testCommentsFile);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public async Task GetCommentAsync_WhenFileDoesNotExist_ShouldReturnEmpty()
    {
        // Arrange & Act
        var result = await _commentService.GetCommentAsync("TEST_VAR");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SetCommentAsync_ShouldSaveCommentToFile()
    {
        // Arrange
        string variableName = "TEST_VAR";
        string comment = "Test comment";

        // Act
        await _commentService.SetCommentAsync(variableName, comment);
        var result = await _commentService.GetCommentAsync(variableName);

        // Assert
        result.Should().Be(comment);
        
        // Проверяем, что файл создан и содержит правильные данные
        File.Exists(_testCommentsFile).Should().BeTrue();
        var jsonContent = await File.ReadAllTextAsync(_testCommentsFile);
        var comments = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
        comments.Should().NotBeNull();
        comments![variableName].Should().Be(comment);
    }

    [Fact]
    public async Task SetCommentAsync_WithEmptyComment_ShouldRemoveEntry()
    {
        // Arrange
        string variableName = "TEST_VAR";
        await _commentService.SetCommentAsync(variableName, "Initial comment");

        // Act
        await _commentService.SetCommentAsync(variableName, "");
        var result = await _commentService.GetCommentAsync(variableName);

        // Assert
        result.Should().BeEmpty();
        
        // Проверяем, что запись удалена из файла
        var jsonContent = await File.ReadAllTextAsync(_testCommentsFile);
        var comments = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
        comments.Should().NotBeNull();
        comments!.ContainsKey(variableName).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCommentAsync_ShouldRemoveComment()
    {
        // Arrange
        string variableName = "TEST_VAR";
        await _commentService.SetCommentAsync(variableName, "Test comment");

        // Act
        await _commentService.DeleteCommentAsync(variableName);
        var result = await _commentService.GetCommentAsync(variableName);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCommentAsync_WhenFileExists_ShouldLoadCommentsFromFile()
    {
        // Arrange
        var testData = new Dictionary<string, string>
        {
            ["VAR1"] = "Comment 1",
            ["VAR2"] = "Comment 2"
        };
        var jsonContent = JsonSerializer.Serialize(testData, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_testCommentsFile, jsonContent);

        // Создаем новый экземпляр сервиса для проверки загрузки
        var newService = new CommentService();
        var fieldInfo = typeof(CommentService).GetField("_commentsFilePath", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo?.SetValue(newService, _testCommentsFile);
        
        // Принудительно вызываем загрузку комментариев
        var loadMethod = typeof(CommentService).GetMethod("LoadComments", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        loadMethod?.Invoke(newService, null);

        // Act
        var result1 = await newService.GetCommentAsync("VAR1");
        var result2 = await newService.GetCommentAsync("VAR2");
        var result3 = await newService.GetCommentAsync("VAR3");

        // Assert
        result1.Should().Be("Comment 1");
        result2.Should().Be("Comment 2");
        result3.Should().BeEmpty();
    }

    [Fact]
    public async Task MultipleOperations_ShouldHandleConcurrently()
    {
        // Arrange
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var index = i;
            tasks.Add(_commentService.SetCommentAsync($"VAR_{index}", $"Comment_{index}"));
        }
        
        await Task.WhenAll(tasks);

        // Assert
        for (int i = 0; i < 10; i++)
        {
            var result = await _commentService.GetCommentAsync($"VAR_{i}");
            result.Should().Be($"Comment_{i}");
        }
    }

    [Fact]
    public async Task SetCommentAsync_WithNullVariableName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _commentService.SetCommentAsync(null!, "comment"));
    }

    [Fact]
    public async Task GetCommentAsync_WithNullVariableName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _commentService.GetCommentAsync(null!));
    }
}
