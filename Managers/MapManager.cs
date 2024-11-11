using System.Collections.Generic;
using static SlipThrough2.Constants;
using SlipThrough2.World;
using SlipThrough2.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Linq;

namespace SlipThrough2
{
    public class MapManager
    {
        // Stores all available maps in a dictionary
        private readonly Dictionary<string, int[,][,]> allMaps = new();
        private readonly Dictionary<string, int[,]> allFunctionalMaps = new();

        public Map Map { get; private set; }

        // Constructor to initialize maps
        public MapManager(List<Texture2D> mapTextures)
        {
            LoadMaps();
            Map = new Map(mapTextures);
            SetMap("mainMap");
        }

        public void Update(Player Player)
        {
            // Player cell position in grid
            int PCX = (int)Player.position.X / CELL_SIZE;
            int PCY = (int)Player.position.Y / CELL_SIZE;

            // Top, right, down, left
            // (PCY > 0) is so that when the player is at index 0 (upper most cell) then we won't check cell at X=-1 since it doesn't exist 
            Player.availableMoves = new bool[4] {
                (PCY > 0) && Map.currentFunctionalPattern[PCY - 1, PCX] == 1 || (PCY > 0) && Map.currentFunctionalPattern[PCY - 1, PCX] == 2,
                (PCX < MAP_WIDTH - 1) && Map.currentFunctionalPattern[PCY, PCX + 1] == 1,
                (PCY < MAP_HEIGHT - 1)  && Map.currentFunctionalPattern[PCY + 1, PCX] == 1,
                (PCX > 0) && Map.currentFunctionalPattern[PCY, PCX - 1] == 1
            };

            if (Map.currentFunctionalPattern[PCY, PCX] == 2)
            {
                Debug.WriteLine("Enter a room");
                SetMap("Map2");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
        }

        // Load maps into the dictionary
        private void LoadMaps()
        {
            // Assign the MAP_ROOM_PATTERN to a name: mainMap in dictionary of all maps
            allMaps["mainMap"] = MAP_ROOM_PATTERN;
            allFunctionalMaps["mainMap"] = GenerateFunctionalMapPattern(allMaps["mainMap"]);

            // Here temporarily, change to something with sense and move to Constants.cs 
            allMaps["Map2"] = MAP_ROOM_PATTERN_FIRST_ENCOUNTER;            
            allFunctionalMaps["Map2"] = GenerateFunctionalMapPattern(allMaps["Map2"]);
        }

        private static int[,] GenerateFunctionalMapPattern(int[,][,] pattern)
        {
            // Mapping tile map to a functional map 
            Debug.WriteLine("Generating Functional Map Pattern");
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

        private void SetMap(string mapName)
        {
            Map.currentPattern = allMaps[mapName];
            Map.currentFunctionalPattern = allFunctionalMaps[mapName];
        }
    }
}