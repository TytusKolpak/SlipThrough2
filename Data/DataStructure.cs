using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// There is a tool online to convert a json structure to cs classes (https://json2csharp.com/)
namespace SlipThrough2.Data
{
    public class DataStructure
    {
        public static DataStructure _constants;
        public Tiles Tiles { get; set; }
        public Settings Settings { get; set; }
        public Maps Maps { get; set; }
        public ViewsStructures Views { get; set; }
        public EntityStructures Entities { get; set; }
        public Encounters Encounters { get; set; }

        public static void LoadJsonData()
        {
            string sourceFilePath = "Content/Data.json";
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException($"The file '{sourceFilePath}' was not found.");

            _constants = JsonSerializer.Deserialize<DataStructure>(
                File.ReadAllText(sourceFilePath)
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
        public Tile[] Enemy { get; set; }
    }

    public class Settings
    {
        public int CellSize { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public int RoomSize { get; set; }
        public int TimeModifierConstant { get; set; }
        public int FontSize { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        // WindowWidth = ColumnCount * RoomSize * CellSize;
        // WindowHeight = RowCount * RoomSize * CellSize;
        // MapWidth = ColumnCount * RoomSize;
        // MapHeight = RowCount * RoomSize;
        public List<TrackedKey> TrackedKeys { get; set; }
    }

    public class TrackedKey
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class Maps
    {
        public Map Main { get; set; }
        public Map EasyEncounter { get; set; }
    }

    public class Map
    {
        public string Name { get; set; }
        public Rooms Rooms { get; set; }
        public string[][] RoomPattern { get; set; } // Can't be string[,]
    }

    public class Rooms
    {
        // OLD = Open Left Down (all abbreviations with letter O are for rooms forming a path with 2 sides open)
        // L-Left, R-Right, D-Down, U-Up
        // LUE = Left Upper Edge (all abbreviations with letter E are for rooms forming confined space with some Edges "closed")
        // L-Left, R-Right, D-Down, U-Upper
        public string[][] OLD { get; set; }
        public string[][] OR { get; set; }
        public string[][] OLU { get; set; }
        public string[][] ORU { get; set; }
        public string[][] OL { get; set; }
        public string[][] ORD { get; set; }
        public string[][] ORL { get; set; }
        public string[][] LUE { get; set; }
        public string[][] RUE { get; set; }
        public string[][] UE { get; set; }
        public string[][] C { get; set; }
        public string[][] LE { get; set; }
        public string[][] RE { get; set; }
        public string[][] DE { get; set; }
        public string[][] LDE { get; set; }
        public string[][] RDE { get; set; }
    }

    public class ViewsStructures
    {
        public ViewStructure StartScreen { get; set; }
        public ViewStructure MainGame { get; set; }
        public ViewStructure Options { get; set; }
    }

    public class ViewStructure
    {
        public string Name { get; set; }
    }

    public class EntityStructures
    {
        public EntityStructure Player { get; set; }
        public EntityStructure EasyEnemy { get; set; }
        public EntityStructure MediumEnemy { get; set; }
        public EntityStructure HardEnemy { get; set; }
    }

    public class EntityStructure
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int Attack { get; set; }
        public int Speed { get; set; }
    }

    public class Encounters
    {
        public string[][] EnemySet { get; set; }
    }
}
