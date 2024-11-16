using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using SlipThrough2.Managers;
using SlipThrough2.World;
using static SlipThrough2.Constants;

namespace SlipThrough2
{
    public class MapManager
    {
        // Stores all available maps in a dictionary
        private readonly Dictionary<MAP_NAME, int[,][,]> allMaps = new();
        private readonly Dictionary<MAP_NAME, int[,]> allFunctionalMaps = new();

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
            // (PCY > 0) is so that when the player is at index 0 (upper most cell) then we won't check cell at X=-1 since it doesn't exist
            Player.availableMoves = new bool[4]
            {
                (PCY > 0) && MapHandler.currentFunctionalPattern[PCY - 1, PCX] == 1
                    || (PCY > 0) && MapHandler.currentFunctionalPattern[PCY - 1, PCX] == 2,
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
            allFunctionalMaps[MAP_NAME.Main] = GenerateFunctionalMapPattern(allMaps[MAP_NAME.Main]);

            // Here temporarily, change to something with sense and move to Constants.cs
            allMaps[MAP_NAME.Encounter1] = MAP_ROOM_PATTERN_FIRST_ENCOUNTER;
            allFunctionalMaps[MAP_NAME.Encounter1] = GenerateFunctionalMapPattern(
                allMaps[MAP_NAME.Encounter1]
            );
        }

        private static int[,] GenerateFunctionalMapPattern(int[,][,] pattern)
        {
            // Mapping tile map to a functional map
            Debug.WriteLine("Generating Functional MapHandler Pattern");
            int[,] functionalPattern = new int[MAP_HEIGHT, MAP_WIDTH];

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
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 2;
                            else
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 0;
                        }
                    }
                }
            }
            return functionalPattern;
        }

        private void SetMap(MAP_NAME mapName)
        {
            MapHandler.currentPattern = allMaps[mapName];
            MapHandler.currentFunctionalPattern = allFunctionalMaps[mapName];
            MapHandler.roomName = mapName;
        }

        private void CheckIfEnteringDoor(int PCY, int PCX, Player Player)
        {
            if (MapHandler.currentFunctionalPattern[PCY, PCX] == 2)
            {
                SetMap(MAP_NAME.Encounter1);

                GameManager.SpawnEnemies();
                Player.HUD.ArrangeTextures();
                Player.position = new(
                    Player.position.X,
                    WINDOW_HEIGHT - Player.position.Y - CELL_SIZE * 1
                );
            }
        }
    }
}
