using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Models;
using EnvironmentVarsApp.Application.Services;
using Moq;

namespace EnvironmentVarsApp.Tests.Services;

/// <summary>
/// Тесты для EnvironmentVariableManagerService
/// </summary>
public class EnvironmentVariableManagerServiceTests
{
    private readonly Mock<IEnvironmentVariableService> _mockEnvironmentVariableService;
    private readonly Mock<IConfigurationService> _mockConfigurationService;
    private readonly EnvironmentVariableManagerService _managerService;

    public EnvironmentVariableManagerServiceTests()
    {
        _mockEnvironmentVariableService = new Mock<IEnvironmentVariableService>();
        _mockConfigurationService = new Mock<IConfigurationService>();
        _managerService = new EnvironmentVariableManagerService(_mockEnvironmentVariableService.Object, _mockConfigurationService.Object);
    }

    [Fact]
    public async Task LoadVariablesAsync_ShouldReturnVariablesFromServices()
    {
        // Arrange
        var variableNames = new[] { "TEST_VAR1", "TEST_VAR2" };
        var expectedVariables = new List<EnvironmentVariable>
        {
            new("TEST_VAR1", "value1", "comment1"),
            new("TEST_VAR2", "value2", "comment2")
        };

        _mockConfigurationService
            .Setup(x => x.GetEnvironmentVariableNamesAsync())
            .ReturnsAsync(variableNames);

        _mockEnvironmentVariableService
            .Setup(x => x.GetEnvironmentVariablesAsync(variableNames))
            .ReturnsAsync(expectedVariables);

        // Act
        var result = await _managerService.LoadVariablesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedVariables);
        
        _mockConfigurationService.Verify(x => x.GetEnvironmentVariableNamesAsync(), Times.Once);
        _mockEnvironmentVariableService.Verify(x => x.GetEnvironmentVariablesAsync(variableNames), Times.Once);
    }

    [Fact]
    public async Task SaveVariableAsync_ShouldCallEnvironmentVariableService()
    {
        // Arrange
        var variable = new EnvironmentVariable("TEST_VAR", "test_value", "test_comment");

        // Act
        await _managerService.SaveVariableAsync(variable);

        // Assert
        _mockEnvironmentVariableService.Verify(
            x => x.SetEnvironmentVariableAsync(variable), 
            Times.Once);
    }

    [Fact]
    public async Task DeleteVariableAsync_ShouldCallEnvironmentVariableService()
    {
        // Arrange
        string variableName = "TEST_VAR";

        // Act
        await _managerService.DeleteVariableAsync(variableName);

        // Assert
        _mockEnvironmentVariableService.Verify(
            x => x.DeleteEnvironmentVariableAsync(variableName), 
            Times.Once);
    }

    [Fact]
    public async Task LoadVariablesAsync_WhenConfigurationServiceThrows_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Configuration error");
        
        _mockConfigurationService
            .Setup(x => x.GetEnvironmentVariableNamesAsync())
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _managerService.LoadVariablesAsync());
        
        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task LoadVariablesAsync_WhenEnvironmentServiceThrows_ShouldPropagateException()
    {
        // Arrange
        var variableNames = new[] { "TEST_VAR" };
        var expectedException = new InvalidOperationException("Environment error");
        
        _mockConfigurationService
            .Setup(x => x.GetEnvironmentVariableNamesAsync())
            .ReturnsAsync(variableNames);

        _mockEnvironmentVariableService
            .Setup(x => x.GetEnvironmentVariablesAsync(variableNames))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _managerService.LoadVariablesAsync());
        
        exception.Should().Be(expectedException);
    }
}
