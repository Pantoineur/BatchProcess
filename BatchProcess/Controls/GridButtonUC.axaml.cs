using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BatchProcess.Models;

namespace BatchProcess.Controls;

public partial class GridButtonUC : UserControl
{
    private ICommand _paintCommand;

    public static readonly DirectProperty<GridButtonUC, ICommand> PaintCommandProperty = AvaloniaProperty.RegisterDirect<GridButtonUC, ICommand>(
        nameof(PaintCommand), o => o.PaintCommand, (o, v) => o.PaintCommand = v);

    public ICommand PaintCommand
    {
        get => _paintCommand;
        set => SetAndRaise(PaintCommandProperty, ref _paintCommand, value);
    }
    
    private Tile _tileInstance;

    public static readonly DirectProperty<GridButtonUC, Tile> TileInstanceProperty = AvaloniaProperty.RegisterDirect<GridButtonUC, Tile>(
        nameof(TileInstance), o => o.TileInstance, (o, v) => o.TileInstance = v);

    public Tile TileInstance
    {
        get => _tileInstance;
        set => SetAndRaise(TileInstanceProperty, ref _tileInstance, value);
    }
    
    public GridButtonUC()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed)
        {
            PaintCommand?.Execute(TileInstance);
        }
    }
}