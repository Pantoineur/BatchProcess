using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using BatchProcess.Data;
using BatchProcess.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess.ViewModels;

public partial class MapCreatorPageViewModel : PageViewModel
{
    public MapCreatorPageViewModel()
    {
        PageName = PageName.MapCreator;
    }

    public MapCreatorPageViewModel(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        Tiles = [];
        
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                Tiles.Add(new()
                {
                    X = x * TileSize,
                    Y = y * TileSize,
                });
            }
        }
    }

    [ObservableProperty]
    private int _tileSize;

    [ObservableProperty]
    private int _rows;
    
    [ObservableProperty]
    private int _columns;

    public ObservableCollection<Tile> Tiles {get; private set;}
}