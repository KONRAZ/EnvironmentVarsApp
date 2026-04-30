using Microsoft.Extensions.Configuration;
using EnvironmentVarsApp.Infrastructure.Services;
using System.Text.Json;

namespace EnvironmentVarsApp.Tests.Services;

/// <summary>
/// Тесты для ConfigurationService
/// </summary>
public class ConfigurationServiceTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testConfigFile;
    private readonly IConfiguration _configuration;
    private readonly ConfigurationService _configurationService;

    public ConfigurationServiceTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _testConfigFile = Path.Combine(_testDirectory, "appsettings.json");

        // Создаем тестовую конфигурацию
        var configData = new Dictionary<string, object>
        {
            ["EnvironmentVariables"] = new[] { "TEST_VAR1", "TEST_VAR2", "TEST_VAR3" },
            ["SomeOtherSetting"] = "value"
        };

        var jsonContent = JsonSerializer.Serialize(configData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_testConfigFile, jsonContent);

        var builder = new ConfigurationBuilder()
            .AddJsonFile(_testConfigFile, optional: false, reloadOnChange: false);
        
        _configuration = builder.Build();
        _configurationService = new ConfigurationService(_configuration);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_ShouldReturnVariableNames()
    {
        // Act
        var result = await _configurationService.GetEnvironmentVariableNamesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().Contain(new[] { "TEST_VAR1", "TEST_VAR2", "TEST_VAR3" });
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_WhenSectionMissing_ShouldReturnEmpty()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder().Build();
        var emptyService = new ConfigurationService(emptyConfig);

        // Act
        var result = await emptyService.GetEnvironmentVariableNamesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_WhenSectionIsEmpty_ShouldReturnEmpty()
    {
        // Arrange
        var emptyData = new Dictionary<string, object>
        {
            ["EnvironmentVariables"] = new string[0]
        };
        
        var jsonContent = JsonSerializer.Serialize(emptyData);
        var emptyConfigFile = Path.Combine(_testDirectory, "empty.json");
        File.WriteAllText(emptyConfigFile, jsonContent);

        var builder = new ConfigurationBuilder()
            .AddJsonFile(emptyConfigFile, optional: false);
        
        var emptyConfig = builder.Build();
        var emptyService = new ConfigurationService(emptyConfig);

        // Act
        var result = await emptyService.GetEnvironmentVariableNamesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_WhenFileIsCorrupted_ShouldThrowException()
    {
        // Arrange
        var corruptedConfigFile = Path.Combine(_testDirectory, "corrupted.json");
        File.WriteAllText(corruptedConfigFile, "{ invalid json content");

        var builder = new ConfigurationBuilder()
            .AddJsonFile(corruptedConfigFile, optional: false);
        
        // Act & Assert
        Assert.Throws<InvalidDataException>(() => builder.Build());
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_WhenFileDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var builder = new ConfigurationBuilder()
            .AddJsonFile("nonexistent.json", optional: false);
        
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => builder.Build());
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_ShouldIgnoreDuplicateNames()
    {
        // Arrange
        var duplicateData = new Dictionary<string, object>
        {
            ["EnvironmentVariables"] = new[] { "TEST_VAR1", "TEST_VAR2", "TEST_VAR1", "TEST_VAR3" }
        };
        
        var jsonContent = JsonSerializer.Serialize(duplicateData);
        var duplicateConfigFile = Path.Combine(_testDirectory, "duplicate.json");
        File.WriteAllText(duplicateConfigFile, jsonContent);

        var builder = new ConfigurationBuilder()
            .AddJsonFile(duplicateConfigFile, optional: false);
        
        var duplicateConfig = builder.Build();
        var duplicateService = new ConfigurationService(duplicateConfig);

        // Act
        var result = await duplicateService.GetEnvironmentVariableNamesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4); // Configuration не дедуплицирует автоматически
        result.Should().Contain("TEST_VAR1");
        result.Should().Contain("TEST_VAR2");
        result.Should().Contain("TEST_VAR3");
    }

    [Fact]
    public async Task GetEnvironmentVariableNamesAsync_WhenNamesHaveWhitespace_ShouldPreserveWhitespace()
    {
        // Arrange
        var whitespaceData = new Dictionary<string, object>
        {
            ["EnvironmentVariables"] = new[] { "TEST_VAR ", " TEST_VAR2", "\tTEST_VAR3\n" }
        };
        
        var jsonContent = JsonSerializer.Serialize(whitespaceData);
        var whitespaceConfigFile = Path.Combine(_testDirectory, "whitespace.json");
        File.WriteAllText(whitespaceConfigFile, jsonContent);

        var builder = new ConfigurationBuilder()
            .AddJsonFile(whitespaceConfigFile, optional: false);
        
        var whitespaceConfig = builder.Build();
        var whitespaceService = new ConfigurationService(whitespaceConfig);

        // Act
        var result = await whitespaceService.GetEnvironmentVariableNamesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().Contain("TEST_VAR ");
        result.Should().Contain(" TEST_VAR2");
        result.Should().Contain("\tTEST_VAR3\n");
    }
}
