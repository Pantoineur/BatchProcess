using System;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BatchProcess.ViewModels;

namespace BatchProcess.Views;

public partial class MapCreatorPageView : UserControl
{
    public MapCreatorPageView()
    {
        InitializeComponent();
        
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var rows = 30;
        var columns = 30;
        
        var width = CanvasGrid.Bounds.Width / rows;
        var height = CanvasGrid.Bounds.Height / columns;

        Console.WriteLine(CanvasGrid.Bounds.Width);

        if (CanvasGrid.Children.Count > 0)
        {
            CanvasGrid.Children.Clear();
        }

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var rng = RandomNumberGenerator.GetInt32(0,2);
                
                Rectangle rectangle = new()
                {
                    Width = width,
                    Height = height,
                    Fill = rng == 0 ? Brushes.BlueViolet : Brushes.Red,
                };
                rectangle.PointerEntered += (sender, args) =>
                {
                    rectangle.Fill = rectangle.Fill.Equals(Brushes.BlueViolet) ? Brushes.Red : Brushes.BlueViolet;
                };
                rectangle.SetValue(Canvas.LeftProperty, j * width);
                rectangle.SetValue(Canvas.TopProperty, i * height);
                CanvasGrid.Children.Add(rectangle);
            }
        }
    }


    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
    }
}