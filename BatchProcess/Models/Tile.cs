using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess.Models;

public partial class Tile : ObservableObject
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Content { get; set; } = "";
    
    [ObservableProperty]
    private IBrush _background = Brushes.BlueViolet;
    
    public TileDef? TileDef { get; set; }
}