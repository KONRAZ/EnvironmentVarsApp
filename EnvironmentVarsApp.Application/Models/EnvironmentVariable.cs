using CommunityToolkit.Mvvm.ComponentModel;

namespace EnvironmentVarsApp.Application.Models;

/// <summary>
/// Переменная среды
/// </summary>
public partial class EnvironmentVariable : ObservableObject
{
    /// <summary>
    /// Имя переменной
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Значение переменной
    /// </summary>
    [ObservableProperty]
    private string _value = string.Empty;

    /// <summary>
    /// Комментарий
    /// </summary>
    [ObservableProperty]
    private string _comment = string.Empty;

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
