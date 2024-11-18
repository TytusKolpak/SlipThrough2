using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class MapManager
    {
        // Stores all available maps in a dictionary
        private readonly Dictionary<MAP_NAME, int[,][,]> allMaps = new();
        private readonly Dictionary<MAP_NAME, int[,]> allFunctionalMaps = new();
        public static int doorNumber;

        public MapHandler MapHandler { get; private set; }

        // Constructor to initialize maps
        public MapManager(List<Texture2D> mapTextures)
        {
            LoadMaps();
            MapHandler = new MapHandler(mapTextures);
            SetMap(MAP_NAME.Main);
        }

        public void Update(Player Player)
        {
            // Player cell position in grid
            int PCX = (int)Player.position.X / CELL_SIZE;
            int PCY = (int)Player.position.Y / CELL_SIZE;

            // Top, right, down, left
            // (PCY > 0) is there for the reason that when the player is at index 0
            // (upper most cell) then we can't check cell at X=-1 since it doesn't exist
            // The >= 1  check is for 1 representing a free space and
            // 2,3,4,... for doors (each door has it's unique number)
            Player.availableMoves = new bool[4]
            {
                (PCY > 0) && MapHandler.currentFunctionalPattern[PCY - 1, PCX] >= 1,
                (PCX < MAP_WIDTH - 1) && MapHandler.currentFunctionalPattern[PCY, PCX + 1] == 1,
                (PCY < MAP_HEIGHT - 1) && MapHandler.currentFunctionalPattern[PCY + 1, PCX] == 1,
                (PCX > 0) && MapHandler.currentFunctionalPattern[PCY, PCX - 1] == 1
            };

            CheckIfEnteringDoor(PCY, PCX, Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MapHandler.Draw(spriteBatch);
        }

        // Load maps into the dictionary
        private void LoadMaps()
        {
            // Assign the MAP_ROOM_PATTERN to a name: Main in dictionary of all maps
            allMaps[MAP_NAME.Main] = MAP_ROOM_PATTERN;
            InsertDoorsIntoMap(allMaps[MAP_NAME.Main], new(1, 0), new(2, 0));
            InsertDoorsIntoMap(allMaps[MAP_NAME.Main], new(2, 0), new(4, 0));
            allFunctionalMaps[MAP_NAME.Main] = GenerateFunctionalMapPattern(allMaps[0]);

            // The same for the second map (Encounter1)
            allMaps[MAP_NAME.Encounter1] = MAP_ROOM_PATTERN_FIRST_ENCOUNTER;
            allFunctionalMaps[MAP_NAME.Encounter1] = GenerateFunctionalMapPattern(
                allMaps[MAP_NAME.Encounter1]
            );
        }

        private static void InsertDoorsIntoMap(int[,][,] map, Point room, Point tile)
        {
            // Clone the room's array to create a separate instance
            // With .Clone() we need to "recast" the now-object into correct type
            map[room.Y, room.X] = (int[,])map[room.Y, room.X].Clone();

            int[,] shorthandForRoom = map[room.Y, room.X];
            shorthandForRoom[tile.Y, tile.X - 1] = 17;
            shorthandForRoom[tile.Y, tile.X] = 16;
            shorthandForRoom[tile.Y, tile.X + 1] = 18;
        }

        private void SetMap(MAP_NAME mapName)
        {
            MapHandler.currentPattern = allMaps[mapName];
            MapHandler.currentFunctionalPattern = allFunctionalMaps[mapName];
            MapHandler.mapName = mapName;
        }

        private void CheckIfEnteringDoor(int PCY, int PCX, Player Player)
        {
            int tileValue = MapHandler.currentFunctionalPattern[PCY, PCX];

            // Doors are 2 and up (each next one has value increased by 1)
            if (tileValue < 2)
                return;

            // Choose which map to enter based room number, but first make
            // the room numbers start from 1 by lowering tile value by 1
            doorNumber = tileValue - 1;

            // The map can be accessed by casting the "index" of this map name
            // in the enum to the type of the enum as: SetMap((MAP_NAME)doorNumber);
            // if there will be more room types then use this way
            SetMap(MAP_NAME.Encounter1);

            EnemyManager.SpawnEnemies(doorNumber);
            HUD.BuildTexturesForBars();

            Player.position = new(
                Player.position.X,
                WINDOW_HEIGHT - Player.position.Y - CELL_SIZE * 2
            );
        }

        private static int[,] GenerateFunctionalMapPattern(int[,][,] pattern)
        {
            // Mapping tile map to a functional map
            int[,] functionalPattern = new int[MAP_HEIGHT, MAP_WIDTH];
            int doorCounter = 0;

            // For every room in the map
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COLUMN_COUNT; x++)
                {
                    // For every tile in the room
                    for (int yt = 0; yt < ROOM_SIZE; yt++)
                    {
                        for (int xt = 0; xt < ROOM_SIZE; xt++)
                        {
                            // It's 1 (as in true) if the tile can be stood on, or 0 if not because it's a wall or a chest
                            // I leave it as int for now because later i will likely want to add chest as 2, door as 3 and so on
                            if (STEPPABLE_TILES.Contains(pattern[y, x][yt, xt]))
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 1;
                            else if (pattern[y, x][yt, xt] == 16)
                            {
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] =
                                    2 + doorCounter;
                                doorCounter++;
                            }
                            else
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 0;
                        }
                    }
                }
            }
            return functionalPattern;
        }
    }
}
