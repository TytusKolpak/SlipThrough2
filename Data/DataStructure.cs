using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SlipThrough2.Data
{
    public class ConstantsModel
    {
        public static ConstantsModel _constants;
        public Tiles Tiles { get; set; }
        public Settings Settings { get; set; }

        public static void LoadJsonData()
        {
            _constants = JsonSerializer.Deserialize<ConstantsModel>(
                File.ReadAllText("Data/Data.json")
            );
        }
    }

    public class Tile
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class FloorTile : Tile
    {
        public bool IsSteppable { get; set; }
        public bool IsDoor { get; set; }
    }

    public class Tiles
    {
        public string PlayerPath { get; set; }
        public List<Tile> Potion { get; set; }
        public List<FloorTile> Floor { get; set; }
        public List<Tile> Enemy { get; set; }
    }

    public class Settings
    {
        public int CellSize { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public int RoomSize { get; set; }
        public int IterationTime { get; set; }
        public int FontSize { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        // WindowWidth = ColumnCount * RoomSize * CellSize;
        // WindowHeight = RowCount * RoomSize * CellSize;
        // MapWidth = ColumnCount * RoomSize;
        // MapHeight = RowCount * RoomSize;
    }
}
