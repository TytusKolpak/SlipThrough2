using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;

namespace SlipThrough2.Managers
{
    public class MapManager
    {
        private static readonly List<FloorTile> floorData = DataStructure._constants.Tiles.Floor;
        private static readonly string[][] enemyData = DataStructure._constants.Encounters.EnemySet;
        private static readonly Settings settingsData = DataStructure._constants.Settings;
        private static readonly Maps mapsData = DataStructure._constants.Maps;
        private static int stage,
            iteration;
        private static bool encounterDoorOpened;
        public static readonly Dictionary<string, string[][]> allMapRoomPatterns = new(); // For how it is build
        public static readonly Dictionary<string, string[,]> allMapTileLayouts = new(); // For how it looks
        public static readonly Dictionary<string, int[,]> allFunctionalMaps = new(); // For how it works
        public static int doorNumber;
        public static bool newMappingApplied = true;
        public MapHandler MapHandler;

        // Constructor to initialize maps
        public MapManager(List<Texture2D> mapTextures)
        {
            LoadMaps();
            MapHandler = new MapHandler(mapTextures);

            if (newMappingApplied)
                GenerateNewTypeMap();

            string mapToSet = newMappingApplied ? mapsData.NewMain.Name : mapsData.Main.Name;
            SetMap(mapToSet);
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

        public static void ChangeMainMapType()
        {
            newMappingApplied = !newMappingApplied;

            if (newMappingApplied)
                GenerateNewTypeMap();
            else
                SetMap(mapsData.Main.Name);
        }

        private static void CheckIfEnteringDoor(int PCY, int PCX, Player Player)
        {
            int tileValue = MapHandler.currentFunctionalPattern[PCY, PCX];

            // Doors are 2 and up (each next one has value increased by 1)
            if (tileValue >= 2)
            {
                AudioManager.PlaySoundOnce("door");

                // Choose which map to enter based room number, but first make
                // the room numbers start from 1 by lowering tile value by 1
                doorNumber = tileValue - 1;

                // Create enemies, HUD and reset combat parameters
                EnemyManager.SpawnEnemies(doorNumber);
                HUDManager.iteration = 0; // Reseting this value for "time"keeping
                HUDManager.BuildTexturesForBars();
                CombatHandler.ResetCombatParameters();
                encounterDoorOpened = false;

                // Modify map
                InsertEncounterDoors();

                // Set modified map (and regenerate functional pattern)
                SetMap(mapsData.EasyEncounter.Name);
            }
            else if (tileValue == -1)
            {
                AudioManager.PlaySoundOnce("door");
                // This means the tile is a door in an encounter
                // and the player will return to the main map

                // Door nr 1 has a tile with value 2, door 2 with value 3 and so on
                doorNumber++;

                string currentMainMap = newMappingApplied
                    ? mapsData.NewMain.Name
                    : mapsData.Main.Name;

                Vector2 doorPosition = FindPositionOfDoors(currentMainMap, doorNumber);

                // Modify the map (close the doors)
                CloseMapDoors(currentMainMap, doorPosition);

                // Set modified map (and regenerate functional pattern)
                SetMap(currentMainMap);

                // Set player position to 1 tile under the door in which they were
                Player.position = doorPosition + new Vector2(0, settingsData.CellSize);

                // And make them move down
                // (otherwise they would keep moving up despite being in the main map)
                Player.direction = new(0, 1);
            }
        }

        private static List<Vector2> ChooseEncounterPositions()
        {
            int numberOfEncounters = DataStructure._constants.Encounters.EnemySet.Length,
                minimumDistance = 5;
            List<Vector2> encounterDoorPositions = new();
            Random rnd = new();

            for (int i = 0; i < numberOfEncounters; i++)
            {
                // 1 is so that the building will not have its doors outside
                // and - 2 is so that there is extra cell for player to fit in from the bottom
                float x = (float)Math.Round(rnd.NextSingle() * (settingsData.MapWidth - 1));
                float y = (float)Math.Round(rnd.NextSingle() * (settingsData.MapHeight - 2));
                Vector2 tempNewPosition = new(x, y);
                bool newPointIsValid = true;

                // Check the new point against all the previous ones
                foreach (Vector2 doorPosition in encounterDoorPositions)
                {
                    float distance = Vector2.Distance(doorPosition, tempNewPosition);

                    // Check if the new point is too close of any of the previous ones
                    if (distance < minimumDistance)
                    {
                        newPointIsValid = false;
                        break;
                    }
                }

                // If the new point is cool then use it, if not find new random one
                if (newPointIsValid)
                    encounterDoorPositions.Add(tempNewPosition);
                else
                    i--;
            }

            encounterDoorPositions.Sort(
                (a, b) => // return 1 if a should come after b. -1 if order is correct
                    a.Y != b.Y // If the 2 values of Y are different then (one is higher than the other)
                        ? (a.Y > b.Y ? 1 : -1) // put the larger at the end of the list (sort ascending)
                        : (a.X > b.X ? 1 : -1) // compare the values of X (they are in the same row)
            );

            return encounterDoorPositions;
        }

        private static void CloseMapDoors(string name, Vector2 doorPosition)
        {
            doorPosition /= settingsData.CellSize;
            string[,] layout = allMapTileLayouts[name];

            layout[(int)doorPosition.Y, (int)doorPosition.X] = "Ds0v1";
        }

        private static Vector2 FindPositionOfDoors(string name, int doorNumber)
        {
            int xIndex = -1,
                yIndex = -1;
            bool found = false;
            int[,] functionalPattern = allFunctionalMaps[name];

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
                                mapName == mapsData.EasyEncounter.Name ? -1 : 2 + doorCounter;
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
            // ShowFunctionalPattern(functionalPattern);
            return functionalPattern;
        }

        public static void GenerateNewTypeMap()
        {
            // In short this map is generated in  much different, random way so it needs it's own function
            Console.WriteLine("Generating new type map");

            // Select the position od encounter buildings at random
            List<Vector2> encounterPositions = ChooseEncounterPositions();
            string name = mapsData.NewMain.Name,
                sandTile = "To0v4",
                sandWithRocksTile = "To0v5";

            // Create random base layout for the newMain map - blank but primed with sand map (+ random rocks)
            string[,] newMainLayout = new string[settingsData.MapHeight, settingsData.MapWidth];
            Random rnd = new();
            for (int y = 0; y < settingsData.MapHeight; y++)
            {
                for (int x = 0; x < settingsData.MapWidth; x++)
                {
                    if (rnd.NextSingle() < 0.75)
                        newMainLayout[y, x] = sandTile; // 75% for normal tile
                    else
                        newMainLayout[y, x] = sandWithRocksTile; // 25% for rocks
                }
            }

            // Build a encounter's building data structure for every encounter
            string[][] encounterBuildingLayout = mapsData.NewMain.EncounterBuilding;

            // Put the buildings on the layout
            for (int i = 0; i < encounterPositions.Count; i++)
            {
                Vector2 offsetForDoorsInBuilding = new(-2, -3);
                Vector2 doorPosition = encounterPositions[i] + offsetForDoorsInBuilding;
                Console.WriteLine(doorPosition);
                for (int y = 0; y < encounterBuildingLayout.Length; y++)
                {
                    for (int x = 0; x < encounterBuildingLayout[0].Length; x++)
                    {
                        // 3 and 2 are here so the position is of the building doors,
                        // not its top left corner. Just the specificity of the room.
                        int adjustedX = (int)doorPosition.X + x;
                        int adjustedY = (int)doorPosition.Y + y;
                        if (
                            adjustedY >= 0
                            && adjustedY < settingsData.MapHeight
                            && adjustedX >= 0
                            && adjustedX < settingsData.MapWidth
                        )
                        {
                            newMainLayout[adjustedY, adjustedX] = encounterBuildingLayout[y][x];

                            // If this is the middle of the building then put an enemy there
                            // Change constants to encounterBuildingLayout derivatives
                            for (int j = 0; j < enemyData[i].Length; j++)
                            {
                                if (y == 1 & x == j + 1)
                                    newMainLayout[adjustedY, adjustedX] = enemyData[i][j];
                            }
                        }
                    }
                }
            }

            // Assign this layout to the name of the map
            allMapTileLayouts[name] = newMainLayout;

            // Set that map as current
            SetMap(name);
        }

        public static dynamic GetObjectsPropertyValue(dynamic objectX, string propertyName)
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

        public static void HandleOpeningDoors()
        {
            if (encounterDoorOpened)
                return;

            iteration++;
            if (iteration == 15 && stage < 3)
            {
                iteration = 0;
                stage++;
                OpenEncounterDoors(stage);
                SetMap(mapsData.EasyEncounter.Name);
            }

            if (stage == 3)
            {
                encounterDoorOpened = true;
                stage = 0;
                CombatHandler.ResetCombatParameters();
            }
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

        private static void LoadMaps()
        {
            // Use data from Data.json as template to create local variables
            string name = mapsData.Main.Name;
            allMapRoomPatterns[name] = mapsData.Main.RoomPattern;

            allMapTileLayouts[name] = TranslateMapPatternToLayout(name);
            InsertDoorsIntoMap(name, new(1, 0), new(2, 0));
            InsertDoorsIntoMap(name, new(2, 0), new(4, 0));

            // The same for the second map (EasyEncounter)
            name = mapsData.EasyEncounter.Name;
            allMapRoomPatterns[name] = mapsData.EasyEncounter.RoomPattern;
            allMapTileLayouts[name] = TranslateMapPatternToLayout(name);
        }

        public static void OpenEncounterDoors(int openingStage)
        {
            Console.WriteLine($"Opening encounter doors, stage: {openingStage}/3.");
            int x = settingsData.MapWidth / 2,
                y = 0;

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

        private static string[,] TranslateMapPatternToLayout(string name)
        {
            Map map = GetObjectsPropertyValue(mapsData, name); // This is now whole EasyEncounter object or MainMap object, found by its name
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
                    string[][] room = GetObjectsPropertyValue(map.Rooms, roomName); // This is its contents
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
    }
}
