using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using BatchProcess.Data;
using BatchProcess.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using System.Runtime.InteropServices;

namespace BatchProcess.ViewModels;

public partial class MapCreatorPageViewModel : PageViewModel
{
    public MapCreatorPageViewModel()
    {
        PageName = PageName.MapCreator;

        Width = 10 * TileSize;
        Height = 10 * TileSize;

        PaintBrush = Brushes.Aqua;
        
        Init(90,10);
    }

    public void Init(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        
        List<Tile> tiles = [];
        CollectionsMarshal.SetCount(tiles, rows * cols);
        
        for(var x = 0; x < Columns; x++)
        {
            for(var y = 0; y < Rows; y++)
            {
                var index = y * Columns + x;
                tiles[index] = new()
                {
                    Y = x * TileSize,
                    X = y * TileSize,
                    TileDef = new() { TileId = 1 } // AFAC 
                };
            }
        }
        
        Tiles = new(tiles);
    }

    public void PointerMoved(Point coordinates)
    {
        var coordX = (int)coordinates.X / TileSize;
        var coordY = (int)coordinates.Y / TileSize;
        var index = coordX * Columns + coordY;

        if (Tiles.Count <= index || index < 0) return;
        
        var tile = Tiles[index];
        tile.Background = PaintBrush;
    }

    [RelayCommand]
    public void SetBrush(int id)
    {
        switch (id)
        {
            case 0:
                PaintBrush = Brushes.Aqua;
                break;
            case 1:
                PaintBrush = Brushes.Blue;
                break;
        }
    }

    private int _currentBrushId = 0;

    [ObservableProperty]
    private int _tileSize = 20;

    [ObservableProperty]
    private int _rows;
    
    [ObservableProperty]
    private int _columns;

    [ObservableProperty]
    private int _width;
    
    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private IBrush _paintBrush; // AFAC ?

    public ObservableCollection<Tile> Tiles { get; set; } = [];
}