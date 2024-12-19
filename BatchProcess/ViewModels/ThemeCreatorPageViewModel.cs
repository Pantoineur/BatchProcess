using System;
using System.Numerics;
using Avalonia.Media;
using BatchProcess.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TowerWarZ.MapCreator.Core;

namespace BatchProcess.ViewModels;

public partial class ThemeCreatorPageViewModel : PageViewModel
{
    private readonly TileDefGenerator _tileDefGenerator;
    
    [ObservableProperty] private Color _backgroundColor = Colors.Chocolate;
    [ObservableProperty] private bool _wireframe;
    [ObservableProperty] private Vector3 _selectedColor;
    [ObservableProperty] private float _hOffset;
    [ObservableProperty] private float _blend = .2f;
    [ObservableProperty] private float _fov = 80f;

    public ThemeCreatorPageViewModel()
    {
        _tileDefGenerator = new();
    }

    partial void OnWireframeChanged(bool value)
    {
        Console.WriteLine($"OnWireframeChanged: {value}");
    }

    partial void OnBackgroundColorChanged(Color oldValue, Color newValue)
    {
        SelectedColor = new(newValue.R/255.0f, newValue.G/255.0f, newValue.B/255.0f);
    }

    partial void OnBackgroundColorChanged(Color value)
    {
        SelectedColor = new(value.R/255.0f, value.G/255.0f, value.B/255.0f);
    }

    public ThemeCreatorPageViewModel(TileDefGenerator tileDefGenerator)
    {
        _tileDefGenerator = tileDefGenerator;
        PageName = PageName.ThemeCreator;
    }

    [RelayCommand]
    public void ToggleWireframe()
    {
        Wireframe = !Wireframe;
    }
}