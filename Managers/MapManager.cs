using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class MapManager
    {
        // Stores all available maps in a dictionary
        public static readonly Dictionary<MAP_NAME, int[,][,]> allMaps = new();
        public static readonly Dictionary<MAP_NAME, int[,]> allFunctionalMaps = new();
        public static int doorNumber;

        public MapHandler MapHandler;

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
            allFunctionalMaps[MAP_NAME.Main] = GenerateFunctionalMapPattern(MAP_NAME.Main);

            // The same for the second map (Encounter1)
            allMaps[MAP_NAME.Encounter1] = MAP_ROOM_PATTERN_FIRST_ENCOUNTER;
            allFunctionalMaps[MAP_NAME.Encounter1] = GenerateFunctionalMapPattern(
                MAP_NAME.Encounter1
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

        private static void InsertEncounterDoors(int[,][,] map)
        {
            Point room = new(2, 0),
                rightCorner = new(5, 0),
                leftCorner = new(0, 0);

            map[room.Y, room.X] = (int[,])map[room.Y, room.X].Clone();
            map[room.Y, room.X + 1] = (int[,])map[room.Y, room.X + 1].Clone();

            int[,] shorthandForRoomLeft = map[room.Y, room.X],
                shorthandForRoomRight = map[room.Y, room.X + 1];

            shorthandForRoomLeft[rightCorner.Y, rightCorner.X - 1] = 17;
            shorthandForRoomLeft[rightCorner.Y, rightCorner.X] = 30;

            shorthandForRoomRight[leftCorner.Y, leftCorner.X] = 31;
            shorthandForRoomRight[leftCorner.Y, leftCorner.X + 1] = 18;
        }

        public static void OpenEncounterDoors(int[,][,] map, int openingStage)
        {
            Point room = new(2, 0),
                rightCorner = new(5, 0),
                leftCorner = new(0, 0);

            map[room.Y, room.X] = (int[,])map[room.Y, room.X].Clone();
            map[room.Y, room.X + 1] = (int[,])map[room.Y, room.X + 1].Clone();

            int[,] shorthandForRoomLeft = map[room.Y, room.X],
                shorthandForRoomRight = map[room.Y, room.X + 1];

            shorthandForRoomLeft[rightCorner.Y, rightCorner.X - 1] = 17;
            shorthandForRoomLeft[rightCorner.Y, rightCorner.X] = 32 + openingStage * 2;

            shorthandForRoomRight[leftCorner.Y, leftCorner.X] = 33 + openingStage * 2;
            shorthandForRoomRight[leftCorner.Y, leftCorner.X + 1] = 18;
        }

        public static void SetMap(MAP_NAME mapName)
        {
            MapHandler.mapName = mapName;
            MapHandler.currentPattern = allMaps[mapName];

            // Regenerate functional pattern based on the new tiles
            MapHandler.currentFunctionalPattern = GenerateFunctionalMapPattern(mapName);
            allFunctionalMaps[mapName] = MapHandler.currentFunctionalPattern;
        }

        private void CheckIfEnteringDoor(int PCY, int PCX, Player Player)
        {
            int tileValue = MapHandler.currentFunctionalPattern[PCY, PCX];

            // Doors are 2 and up (each next one has value increased by 1)
            if (tileValue >= 2)
            {
                // Choose which map to enter based room number, but first make
                // the room numbers start from 1 by lowering tile value by 1
                doorNumber = tileValue - 1;

                // Create enemies and HUD
                EnemyManager.SpawnEnemies(doorNumber);
                HUDManager.BuildTexturesForBars();

                // Modify map
                InsertEncounterDoors(allMaps[MAP_NAME.Encounter1]);

                // Set modified map (and regenerate functional pattern)
                SetMap(MAP_NAME.Encounter1);

                Player.position = new(
                    Player.position.X,
                    WINDOW_HEIGHT - Player.position.Y - CELL_SIZE * 2
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
                SetMap(MAP_NAME.Main);

                // Set player position to 1 tile under the door in which they were
                Player.position = doorPosition + new Vector2(0, CELL_SIZE);
            }
        }

        private static Vector2 FindPositionOfDoors(int doorNumber)
        {
            int xIndex = -1,
                yIndex = -1;
            bool found = false;
            int[,] functionalPattern = allFunctionalMaps[MAP_NAME.Main];

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
            return new Vector2(xIndex, yIndex) * CELL_SIZE;
        }

        private static void CloseMainMapDoors(Vector2 doorPosition)
        {
            doorPosition /= CELL_SIZE;
            Console.WriteLine($"{doorPosition}");
            Point roomPosition =
                new((int)doorPosition.X / ROOM_SIZE, (int)doorPosition.Y / ROOM_SIZE);
            Point positionInRoom =
                new((int)doorPosition.X % ROOM_SIZE, (int)doorPosition.Y % ROOM_SIZE);

            int roomSize = allMaps[MAP_NAME.Main][1, 0].Length;
            int mapSize = allMaps[MAP_NAME.Main].Length;
            Console.WriteLine($"{roomPosition}, {positionInRoom}, {roomSize}, {mapSize}");
            allMaps[MAP_NAME.Main][roomPosition.Y, roomPosition.X][
                positionInRoom.Y,
                positionInRoom.X
            ] = 38;
        }

        public static int[,] GenerateFunctionalMapPattern(MAP_NAME mapName)
        {
            int[,][,] pattern = allMaps[mapName];
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
                            if (STEPPABLE_TILES.Contains(pattern[y, x][yt, xt]))
                            {
                                // It's 1 (as in true) if the tile can be stood on
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 1;
                            }
                            else if (DOOR_TILES.Contains(pattern[y, x][yt, xt]))
                            {
                                // 2 and above means the tile is a door
                                // If current map is an encounter put -1 for returning to main map
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] =
                                    mapName == MAP_NAME.Main ? 2 + doorCounter : -1;
                                doorCounter++;
                            }
                            else
                            {
                                // 0 means the tile can't be stepped on
                                functionalPattern[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = 0;
                            }
                        }
                    }
                }
            }
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
    }
}
