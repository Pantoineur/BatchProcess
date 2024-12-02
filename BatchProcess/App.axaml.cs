using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BatchProcess.Data;
using BatchProcess.Factories;
using BatchProcess.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using TowerWarZ.MapCreator.Core;

namespace BatchProcess;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<PageFactory>();
        collection.AddSingleton<Func<PageName, PageViewModel>>(x => name => name switch
        {
            PageName.Home => x.GetRequiredService<HomePageViewModel>(),
            PageName.MapCreator => x.GetRequiredService<MapCreatorPageViewModel>(),
            PageName.ThemeCreator => x.GetRequiredService<ThemeCreatorPageViewModel>(),
            // PageName.About => expr,
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
        });
        collection.AddSingleton<TileDefGenerator>();
        
        var types = typeof(ViewModelBase).Assembly.GetTypes();
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(ViewModelBase)) && !type.Name.Equals("MainViewModel"))
            {
                collection.AddScoped(type);
            }
        }

        var services = collection.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainView
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}