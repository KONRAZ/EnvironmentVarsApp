using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnvironmentVarsApp.Application.Interfaces;
using EnvironmentVarsApp.Application.Models;
using System.Collections.ObjectModel;

namespace EnvironmentVarsApp.ViewModels;

/// <summary>
/// ViewModel для главного окна
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    private readonly IEnvironmentVariableManagerService _managerService;

    [ObservableProperty]
    private ObservableCollection<EnvironmentVariable> _environmentVariables = new();

    public MainWindowViewModel()
    {
        _managerService = App.GetService<IEnvironmentVariableManagerService>();
        LoadVariablesAsync();
    }

    /// <summary>
    /// Загрузить переменные
    /// </summary>
    private async Task LoadVariablesAsync()
    {
        try
        {
            var variables = await _managerService.LoadVariablesAsync();
            
            EnvironmentVariables.Clear();
            foreach (var variable in variables)
            {
                EnvironmentVariables.Add(variable);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при загрузке переменных: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Сохранить переменную
    /// </summary>
    [RelayCommand]
    private async Task SaveVariableAsync(EnvironmentVariable variable)
    {
        try
        {
            await _managerService.SaveVariableAsync(variable);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при сохранении переменной: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Удалить переменную
    /// </summary>
    [RelayCommand]
    private async Task DeleteVariableAsync(EnvironmentVariable variable)
    {
        try
        {
            await _managerService.DeleteVariableAsync(variable.Name);
            EnvironmentVariables.Remove(variable);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при удалении переменной: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Обновить переменные
    /// </summary>
    [RelayCommand]
    private async Task RefreshVariablesAsync()
    {
        await LoadVariablesAsync();
    }
}
