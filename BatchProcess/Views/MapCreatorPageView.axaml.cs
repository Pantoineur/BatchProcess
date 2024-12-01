using System;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using BatchProcess.ViewModels;

namespace BatchProcess.Views;

public partial class MapCreatorPageView : UserControl
{
    private double _lastWidth = 0;
    private double _lastHeight = 0;
    
    public static bool LeftButtonDown = false;
    public MapCreatorPageView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        
        AddHandler(PointerPressedEvent, (sender, args) =>
        {
            LeftButtonDown = true;
            e.Handled = true;
        }, handledEventsToo: true);
        
        AddHandler(PointerReleasedEvent, (sender, args) =>
        {
            LeftButtonDown = false;
            e.Handled = true;
        }, handledEventsToo: true);
        
        AddHandler(PointerMovedEvent, (sender, args) =>
        {
            e.Handled = true;
            if (LeftButtonDown)
            {
                (DataContext as MapCreatorPageViewModel)?.PointerMoved(args.GetPosition(MapCreatorGrid));
            }
        }, handledEventsToo: true);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (MapCreatorGrid is not null 
            && (MapCreatorGrid.DesiredSize.Width > _lastWidth || MapCreatorGrid.DesiredSize.Width < _lastWidth
            || MapCreatorGrid.DesiredSize.Height > _lastHeight || MapCreatorGrid.DesiredSize.Height < _lastHeight))
        {
            if (MapCreatorGrid.ItemsPanelRoot is not null)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    MapCreatorGrid.ItemsPanelRoot.Width = MapCreatorGrid.DesiredSize.Width;
                    MapCreatorGrid.ItemsPanelRoot.Height = MapCreatorGrid.DesiredSize.Height;
                });
            }
        }
    }
}