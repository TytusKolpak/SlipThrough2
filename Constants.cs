using System.Diagnostics;

namespace SlipThrough2
{
    public static class Constants
    {
        public const int CELL_SIZE = 32;
        public const int COLUMN_COUNT = 6;
        public const int ROW_COUNT = 3;
        public const int ROOM_SIZE = 6; // We assume square rooms for now with 6x6 cell grid
        public const int WINDOW_WIDTH = COLUMN_COUNT * ROOM_SIZE * CELL_SIZE;
        public const int WINDOW_HEIGHT = ROW_COUNT * ROOM_SIZE * CELL_SIZE;
        public const int MAP_WIDTH = COLUMN_COUNT * ROOM_SIZE; // Witdh in Cells
        public const int MAP_HEIGHT = ROW_COUNT * ROOM_SIZE;

        public const int ITERATION_TIME = 15; // Frames (right now there are 60 frames per second)

        // Original tile pattern TPO = Tile patter Open
        public static readonly int[,] TPO_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  2,  1,  0,  0, 11},
            { 7,  0,  0,  0,  0, 0 },
            { 7,  0,  0,  1,  0, 13},
            { 7,  2,  0,  0, 15, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TPO_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            {12,  0,  1,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 8 },
            {14,  0,  0,  0,  0, 8 },
            { 7,  2,  0,  0,  2, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            {12,  0,  1,  0,  0, 11},
            { 0,  0,  0,  0,  0, 0 },
            {14,  0,  0,  0,  0, 13},
            { 7,  1,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TPO_LEFT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            {12,  0,  1,  0,  2, 8 },
            { 0,  0,  0,  0,  2, 8 },
            {14,  0,  1,  1,  0, 8 },
            { 7, 15,  0,  0,  0, 8 },
            { 5,  9, 14,  0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 12,  0, 11, 3 },
            {12,  0,  0,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 8 },
            {14,  0,  1,  1,  0, 8 },
            { 7,  2,  0,  2,  2, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 11},
            { 7,  1,  0,  0,  0, 0 },
            { 7,  0,  0,  0,  0, 13},
            { 7,  0,  0,  0,  0, 8 },
            { 5,  9, 14,  0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 12,  0, 11, 3 },
            { 7,  2,  0,  0,  0, 11},
            { 7, 15,  0,  0,  2, 0 },
            { 7,  0,  1,  0,  0, 13},
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };

        // Create a jagged array of 4 elements, where each element is a reference to TPO_UP_RIGHT
        public static readonly int[,][,] ROOM_PATTERN = new int[ROW_COUNT, COLUMN_COUNT][,]
        {
            {TPO_RIGHT      , TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_LEFT_DOWN },
            {TPO_RIGHT_DOWN , TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_UP_LEFT   },
            {TPO_UP_RIGHT   , TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_RIGHT_LEFT, TPO_LEFT      }
        };

        // Functional room pattern
        // Only tiles with index 0,1 or 2 are available to stand on
        // 3-14 are different types of walls
        // 15 is a chest (can't be stood on but can be "opened" by attempting to stand on)
        public static readonly int[,] FUNCTIONAL_MAP_PATTERN = new int[MAP_HEIGHT, MAP_WIDTH];

        public static void Initialize()
        {
            // Mapping tile map to a functional map 
            Debug.WriteLine("Initialize MAP_FUNCTIONAL_PATTERN");

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
                            FUNCTIONAL_MAP_PATTERN[y * ROOM_SIZE + yt, x * ROOM_SIZE + xt] = ROOM_PATTERN[y, x][yt, xt] < 3 ? 1 : 0;
                        }
                    }
                }
            }
        }
    }
}
