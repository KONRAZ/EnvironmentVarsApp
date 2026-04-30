using EnvironmentVarsApp.Application.Models;

namespace EnvironmentVarsApp.Tests.Models;

/// <summary>
/// Тесты для EnvironmentVariable
/// </summary>
public class EnvironmentVariableTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        string name = "TEST_VAR";
        string value = "test_value";
        string comment = "test_comment";

        // Act
        var variable = new EnvironmentVariable(name, value, comment);

        // Assert
        variable.Name.Should().Be(name);
        variable.Value.Should().Be(value);
        variable.Comment.Should().Be(comment);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EnvironmentVariable(null!, "value", "comment"));
    }

    [Fact]
    public void Constructor_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EnvironmentVariable("name", null!, "comment"));
    }

    [Fact]
    public void Constructor_WithNullComment_ShouldUseEmptyString()
    {
        // Arrange
        string name = "TEST_VAR";
        string value = "test_value";

        // Act
        var variable = new EnvironmentVariable(name, value, null!);

        // Assert
        variable.Name.Should().Be(name);
        variable.Value.Should().Be(value);
        variable.Comment.Should().BeEmpty();
    }

    [Fact]
    public void DefaultConstructor_ShouldCreateInstanceWithEmptyProperties()
    {
        // Act
        var variable = new EnvironmentVariable();

        // Assert
        variable.Name.Should().BeEmpty();
        variable.Value.Should().BeEmpty();
        variable.Comment.Should().BeEmpty();
    }

    [Fact]
    public void ToString_ShouldReturnNameEqualsValueFormat()
    {
        // Arrange
        var variable = new EnvironmentVariable("TEST_VAR", "test_value", "comment");

        // Act
        var result = variable.ToString();

        // Assert
        result.Should().Be("TEST_VAR = test_value");
    }

    [Fact]
    public void Equals_WithSameName_ShouldReturnTrue()
    {
        // Arrange
        var var1 = new EnvironmentVariable("TEST_VAR", "value1", "comment1");
        var var2 = new EnvironmentVariable("TEST_VAR", "value2", "comment2");

        // Act
        var result = var1.Equals(var2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentNameCase_ShouldReturnTrue()
    {
        // Arrange
        var var1 = new EnvironmentVariable("TEST_VAR", "value1", "comment1");
        var var2 = new EnvironmentVariable("test_var", "value2", "comment2");

        // Act
        var result = var1.Equals(var2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentName_ShouldReturnFalse()
    {
        // Arrange
        var var1 = new EnvironmentVariable("TEST_VAR1", "value1", "comment1");
        var var2 = new EnvironmentVariable("TEST_VAR2", "value2", "comment2");

        // Act
        var result = var1.Equals(var2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var variable = new EnvironmentVariable("TEST_VAR", "value", "comment");

        // Act
        var result = variable.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var variable = new EnvironmentVariable("TEST_VAR", "value", "comment");

        // Act
        var result = variable.Equals("string");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameNameCase_ShouldReturnSameHashCode()
    {
        // Arrange
        var var1 = new EnvironmentVariable("TEST_VAR", "value1", "comment1");
        var var2 = new EnvironmentVariable("test_var", "value2", "comment2");

        // Act
        var hash1 = var1.GetHashCode();
        var hash2 = var2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void GetHashCode_WithDifferentName_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var var1 = new EnvironmentVariable("TEST_VAR1", "value1", "comment1");
        var var2 = new EnvironmentVariable("TEST_VAR2", "value2", "comment2");

        // Act
        var hash1 = var1.GetHashCode();
        var hash2 = var2.GetHashCode();

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Constructor_WithWhitespaceName_ShouldCreateInstance(string name)
    {
        // Arrange
        string value = "test_value";
        string comment = "test_comment";

        // Act
        var variable = new EnvironmentVariable(name, value, comment);

        // Assert
        variable.Name.Should().Be(name);
        variable.Value.Should().Be(value);
        variable.Comment.Should().Be(comment);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Constructor_WithWhitespaceValue_ShouldCreateInstance(string value)
    {
        // Arrange
        string name = "TEST_VAR";
        string comment = "test_comment";

        // Act
        var variable = new EnvironmentVariable(name, value, comment);

        // Assert
        variable.Name.Should().Be(name);
        variable.Value.Should().Be(value);
        variable.Comment.Should().Be(comment);
    }
}
