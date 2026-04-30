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

    [ObservableProperty]
    private EnvironmentVariable? _selectedVariable;

    public MainWindowViewModel()
    {
        _managerService = App.GetService<IEnvironmentVariableManagerService>();
        _ = LoadVariablesAsync();
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
    /// Сохранить все переменные
    /// </summary>
    [RelayCommand]
    private async Task SaveAllVariablesAsync()
    {
        if (EnvironmentVariables == null || EnvironmentVariables.Count == 0)
        {
            throw new InvalidOperationException("Нет переменных для сохранения");
        }

        try
        {
            // Принудительно обновляем все привязки перед сохранением
            foreach (var variable in EnvironmentVariables)
            {
                if (string.IsNullOrWhiteSpace(variable.Value))
                {
                    continue;
                }
                await _managerService.SaveVariableAsync(variable);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при сохранении переменных: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Удалить переменную
    /// </summary>
    [RelayCommand]
    private async Task DeleteVariableAsync()
    {
        if (SelectedVariable == null)
        {
            throw new InvalidOperationException("Не выбрана переменная для очистки");
        }

        try
        {
            await _managerService.DeleteVariableAsync(SelectedVariable.Name);
            SelectedVariable.Value = string.Empty;
            SelectedVariable.Comment = string.Empty;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при очистке переменной: {ex.Message}", ex);
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
