using BatchProcess.Data;
using TowerWarZ.MapCreator.Core;

namespace BatchProcess.ViewModels;

public partial class ThemeCreatorPageViewModel : PageViewModel
{
    private readonly TileDefGenerator _tileDefGenerator;

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