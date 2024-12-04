using System.Drawing;
using BatchProcess.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using TowerWarZ.MapCreator.Core;

namespace BatchProcess.ViewModels;

public partial class ThemeCreatorPageViewModel : PageViewModel
{
    private readonly TileDefGenerator _tileDefGenerator;
    
    [ObservableProperty] private Color _backgroundColor = Color.Chocolate;

    public ThemeCreatorPageViewModel()
    {
        _tileDefGenerator = new TileDefGenerator();
    }
    
    public ThemeCreatorPageViewModel(TileDefGenerator tileDefGenerator)
    {
        _tileDefGenerator = tileDefGenerator;
        PageName = PageName.ThemeCreator;
    }
}