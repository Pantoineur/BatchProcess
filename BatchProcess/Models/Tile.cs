namespace BatchProcess.Models;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }

    public string Content { get; set; } = "";

    public TileDef TileDef { get; set; }
}