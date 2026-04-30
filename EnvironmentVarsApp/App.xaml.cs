using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EnvironmentVarsApp.Extensions;
using System.Windows;

namespace EnvironmentVarsApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        // Создаем конфигурацию
        var configuration = ServiceCollectionExtensions.CreateConfiguration();

        // Создаем хост с DI контейнером
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.ConfigureApplicationServices(configuration);
            })
            .Build();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }

    /// <summary>
    /// Получить сервис из DI контейнера
    /// </summary>
    public static T GetService<T>() where T : class
    {
        var app = (App)Current;
        return app._host?.Services.GetService<T>() 
               ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered");
    }
}

