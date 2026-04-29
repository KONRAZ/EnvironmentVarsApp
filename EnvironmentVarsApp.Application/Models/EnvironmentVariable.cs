namespace EnvironmentVarsApp.Application.Models;

/// <summary>
/// Переменная среды
/// </summary>
public class EnvironmentVariable
{
    /// <summary>
    /// Имя переменной
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Значение переменной
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    public EnvironmentVariable() { }

    public EnvironmentVariable(string name, string value, string comment = "")
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Comment = comment ?? string.Empty;
    }

    public override string ToString()
    {
        return $"{Name} = {Value}";
    }

    public override bool Equals(object? obj)
    {
        return obj is EnvironmentVariable variable && Name.Equals(variable.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}
