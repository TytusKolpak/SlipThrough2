﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;

namespace SlipThrough2.Managers
{
    public class MapManager
    {
        public static readonly Dictionary<string, string[][]> allMapRoomPatterns = new(); // For how it is build
        public static readonly Dictionary<string, string[,]> allMapTileLayouts = new(); // For how it looks
        public static readonly Dictionary<string, int[,]> allFunctionalMaps = new(); // For how it works
        public static int doorNumber;
        public MapHandler MapHandler;
        private static List<FloorTile> floorData;
        private static Settings settingsData;
        private static Maps mapsData;

        // Constructor to initialize maps
        public MapManager(List<Texture2D> mapTextures)
        {
            floorData = DataStructure._constants.Tiles.Floor;
            settingsData = DataStructure._constants.Settings;
            mapsData = DataStructure._constants.Maps;

            LoadMaps();
            MapHandler = new MapHandler(mapTextures);
            SetMap(mapsData.Main.Name);
        }

        public void Update(Player Player)
        {
            // Player cell position in grid
            int PCX = (int)Player.position.X / settingsData.CellSize;
            int PCY = (int)Player.position.Y / settingsData.CellSize;
            CheckIfEnteringDoor(PCY, PCX, Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MapHandler.Draw(spriteBatch);
        }

        // Load maps into the dictionary
        private void LoadMaps()
        {
            // Use data from Data.json as template to create local variables
            string name = mapsData.Main.Name;
            allMapRoomPatterns[name] = mapsData.Main.RoomPattern;

            allMapTileLayouts[name] = TranslateMapPatternToLayout(name);
            InsertDoorsIntoMap(name, new(1, 0), new(2, 0));
            InsertDoorsIntoMap(name, new(2, 0), new(4, 0));

            allFunctionalMaps[name] = GenerateFunctionalMapPattern(name);

            // The same for the second map (EasyEncounter)
            name = mapsData.EasyEncounter.Name;
            allMapRoomPatterns[name] = mapsData.EasyEncounter.RoomPattern;
            allMapTileLayouts[name] = TranslateMapPatternToLayout(name);
            allFunctionalMaps[name] = GenerateFunctionalMapPattern(name);
        }

        private static string[,] TranslateMapPatternToLayout(string name)
        {
            Map map = getObjectsPropertyValue(mapsData, name); // This is now whole EasyEncounter object or MainMap object, found by its name
            int patternHeight = settingsData.RowCount;
            int patternWidth = settingsData.ColumnCount;
            int roomHeight = settingsData.RoomSize;
            int roomWidth = settingsData.RoomSize;

            string[,] layout = new string[patternHeight * roomHeight, patternWidth * roomWidth];
            string[][] pattern = map.RoomPattern;

            for (int row = 0; row < patternHeight; row++)
            {
                for (int column = 0; column < patternWidth; column++)
                {
                    string roomName = pattern[row][column]; // As in "OLD" (Open left down which holds string[][])
                    string[][] room = getObjectsPropertyValue(map.Rooms, roomName); // This is its contents
                    for (int y = 0; y < roomHeight; y++)
                    {
                        for (int x = 0; x < roomWidth; x++)
                        {
                            layout[row * roomHeight + y, column * roomWidth + x] = room[y][x]; // This is now "Wo8v1"
                        }
                    }
                }
            }
            // ShowLayout(layout);
            return layout;
        }

        private static void InsertDoorsIntoMap(string name, Point room, Point tile)
        {
            string[,] layout = allMapTileLayouts[name];
            int x = room.X * settingsData.RoomSize + tile.X;
            int y = room.Y * settingsData.RoomSize + tile.Y;
            layout[y, x - 1] = "Wo7v3";
            layout[y, x] = "Ds1v1";
            layout[y, x + 1] = "Wo3v3";
        }

        private static void InsertEncounterDoors()
        {
            int x = settingsData.MapWidth / 2;
            int y = 0;

            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x - 2] = "Wo7v3";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x - 1] = "Do7s0v1";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x] = "Do3s0v1";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x + 1] = "Wo3v3";

            Console.WriteLine("Inserted encounter doors");
        }

        public static void OpenEncounterDoors(int openingStage)
        {
            Console.WriteLine($"Opening encounter doors, stage: {openingStage}/3.");
            int x = settingsData.MapWidth / 2;
            int y = 0;

            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x - 2] = "Wo7v3";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x - 1] = "Do7s" + openingStage + "v1";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x] = "Do3s" + openingStage + "v1";
            allMapTileLayouts[mapsData.EasyEncounter.Name][y, x + 1] = "Wo3v3";
        }

        public static void SetMap(string mapName)
        {
            MapHandler.mapName = mapName;
            MapHandler.currentTileLayout = allMapTileLayouts[mapName];

            // Regenerate functional pattern based on the new tiles
            MapHandler.currentFunctionalPattern = GenerateFunctionalMapPattern(mapName);
            allFunctionalMaps[mapName] = MapHandler.currentFunctionalPattern;

            Console.WriteLine($"Set {mapName} as current map.");
        }

        private static void CheckIfEnteringDoor(int PCY, int PCX, Player Player)
        {
            int tileValue = MapHandler.currentFunctionalPattern[PCY, PCX];

            // Doors are 2 and up (each next one has value increased by 1)
            if (tileValue >= 2)
            {
                // Choose which map to enter based room number, but first make
                // the room numbers start from 1 by lowering tile value by 1
                doorNumber = tileValue - 1;

                // Create enemies, HUD and reset combat parameters
                EnemyManager.SpawnEnemies(doorNumber);
                HUDManager.iteration = 0; // Reseting this value for "time"keeping
                HUDManager.BuildTexturesForBars();
                CombatHandler.ResetCombatParameters();

                // Modify map
                InsertEncounterDoors();

                // Set modified map (and regenerate functional pattern)
                SetMap(mapsData.EasyEncounter.Name);

                Player.position = new(
                    Player.position.X,
                    settingsData.WindowHeight - Player.position.Y - settingsData.CellSize * 2
                );
            }
            else if (tileValue == -1)
            {
                // This means the tile is a door in an encounter
                // and the player will return to the main map

                // Door nr 1 has a tile with value 2, door 2 with value 3 and so on
                doorNumber++;
                Vector2 doorPosition = FindPositionOfDoors(doorNumber);

                // Modify the map (close the doors)
                CloseMainMapDoors(doorPosition);

                // Set modified map (and regenerate functional pattern)
                SetMap(mapsData.Main.Name);

                // Set player position to 1 tile under the door in which they were
                Player.position = doorPosition + new Vector2(0, settingsData.CellSize);
            }
        }

        private static Vector2 FindPositionOfDoors(int doorNumber)
        {
            int xIndex = -1,
                yIndex = -1;
            bool found = false;
            int[,] functionalPattern = allFunctionalMaps[mapsData.Main.Name];

            for (int i = 0; i < functionalPattern.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < functionalPattern.GetLength(1); j++) // Columns
                {
                    if (functionalPattern[i, j] == doorNumber)
                    {
                        xIndex = j;
                        yIndex = i;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break; // Exit outer loop if found
            }
            // This is used to set position so we need to translate it to position from index pair
            return new Vector2(xIndex, yIndex) * settingsData.CellSize;
        }

        private static void CloseMainMapDoors(Vector2 doorPosition)
        {
            doorPosition /= settingsData.CellSize;
            string[,] layout = allMapTileLayouts[mapsData.Main.Name];

            layout[(int)doorPosition.Y, (int)doorPosition.X] = "Ds0v1";
        }

        public static int[,] GenerateFunctionalMapPattern(string mapName)
        {
            string[,] layout = allMapTileLayouts[mapName];
            int[,] functionalPattern = new int[settingsData.MapHeight, settingsData.MapWidth];
            int doorCounter = 0;

            for (int y = 0; y < settingsData.MapHeight; y++)
            {
                for (int x = 0; x < settingsData.MapWidth; x++)
                {
                    string tileCode = layout[y, x];

                    FloorTile tile = floorData.Find(tile => tile.Name == tileCode);
                    if (tile.IsSteppable)
                    {
                        if (tile.IsDoor)
                        {
                            // If current map is an encounter put -1 for returning to main map
                            functionalPattern[y, x] =
                                mapName == mapsData.Main.Name ? 2 + doorCounter : -1;
                            doorCounter++;
                        }
                        else
                        {
                            // Is a standard non functional steppable tile
                            functionalPattern[y, x] = 1;
                        }
                    }
                    else
                    {
                        if (tile.IsDoor) // But closed
                            doorCounter++;

                        // 0 means the tile can't be stepped on
                        functionalPattern[y, x] = 0;
                    }
                }
            }
            Console.WriteLine($"Generated functional map pattern for {mapName}.");
            return functionalPattern;
        }

        public static void ShowFunctionalPattern(int[,] functionalPattern)
        {
            Console.WriteLine("Functional pattern:");
            for (int i = 0; i < functionalPattern.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < functionalPattern.GetLength(1); j++) // Columns
                {
                    Console.Write($"{functionalPattern[i, j], 4}"); // Format spacing
                }
                Console.WriteLine(); // Move to the next row
            }
            Console.WriteLine("End");
        }

        public static void ShowLayout(string[,] layout)
        {
            Console.WriteLine("Layout:");
            for (int i = 0; i < layout.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < layout.GetLength(1); j++) // Columns
                {
                    Console.Write($"{layout[i, j], 7}"); // Format spacing
                }
                Console.WriteLine(); // Move to the next row
            }
            Console.WriteLine("End");
        }

        public static dynamic getObjectsPropertyValue(dynamic objectX, string propertyName)
        {
            if (objectX == null)
                throw new ArgumentNullException(nameof(objectX), "Error: object is null.");

            if (propertyName == null)
                throw new ArgumentNullException(
                    nameof(propertyName),
                    "Error: property name is null."
                );

            var propertyInfo = objectX.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                throw new ArgumentException(
                    $"Error: no such property '{propertyName}' on object {objectX}."
                );

            var value = propertyInfo.GetValue(objectX, null);

            return value;
        }
    }
}
