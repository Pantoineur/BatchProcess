using System.Reflection;

namespace TowerWarZ.MapCreator.Core;

public class TileDefGenerator
{
    private string _tileDefDirectory;
    
    public TileDefGenerator()
    {
        _tileDefDirectory = Path.Join(Assembly.GetExecutingAssembly().Location, "..", "..", "TileDefs");
        Console.WriteLine(_tileDefDirectory);
    }
}