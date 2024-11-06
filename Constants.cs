namespace SlipThrough2
{
    public static class Constants
    {
        public const int CELL_SIZE = 32;
        public const int COLUMN_COUNT = 6;
        public const int ROW_COUNT = 3;
        public const int ROOM_SIZE = 6; // We assume square rooms for now 
        public const int WINDOW_WIDTH = CELL_SIZE * COLUMN_COUNT * ROOM_SIZE;
        public const int WINDOW_HEIGHT = CELL_SIZE * ROW_COUNT * ROOM_SIZE;

        // Original tile pattern
        public static readonly int[,] TILE_PATTERN_OPEN_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 7,  0,  0,  0,  2, 0 },
            { 7,  0,  1,  1,  0, 0 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 8 },
            { 0,  0,  1,  1,  0, 8 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_RIGHT_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 0 },
            { 0,  0,  1,  1,  0, 0 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_LEFT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 8 },
            { 0,  0,  1,  1,  0, 8 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  0,  0,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_UP_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10,  0,  0, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 0,  0,  0,  0,  2, 8 },
            { 0,  0,  1,  1,  0, 8 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_RIGHT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 7,  0,  0,  0,  2, 0 },
            { 7,  0,  1,  1,  0, 0 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  0,  0,  9, 4 }
        };
        public static readonly int[,] TILE_PATTERN_OPEN_UP_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10,  0,  0, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 7,  0,  0,  0,  2, 0 },
            { 7,  0,  1,  1,  0, 0 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };

        // Create a jagged array of 4 elements, where each element is a reference to TILE_PATTERN_OPEN_UP_RIGHT
        public static readonly int[][,] TILE_PATTERNS = new int[COLUMN_COUNT * ROW_COUNT][,]
        {
            (int[,])TILE_PATTERN_OPEN_RIGHT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_LEFT_DOWN.Clone(),

            (int[,])TILE_PATTERN_OPEN_RIGHT_DOWN.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_UP_LEFT.Clone(),

            (int[,])TILE_PATTERN_OPEN_UP_RIGHT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_RIGHT_LEFT.Clone(),
            (int[,])TILE_PATTERN_OPEN_LEFT.Clone(),
        };
    }
}
