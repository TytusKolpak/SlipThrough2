using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

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

        public const int ITERATION_TIME = 10; // Frames (right now there are 60 frames per second)
        public const int FONT_SIZE = 24;

        public static readonly int[] STEPPABLE_TILES = { 0, 1, 2, 19, 20, 21, 22, 23 };
        public static readonly int[] HEALTH_HUD_TILE_PATTERN = { 2, 2, 2, 2, 0 };
        public static readonly int[] MANA_HUD_TILE_PATTERN = { 3, 3, 3, 0, 0 };
        public static readonly Keys[] TRACKED_KEYS = { Keys.Escape, Keys.Enter, Keys.R };

        public enum MAP_NAME
        {
            Main,
            Encounter1
        };

        public enum VIEW_NAME
        {
            StartScreen,
            MainGame,
            Options
        }

        public static readonly string[] TILE_PATHS =
        {
            "Tiles/tile_0000", // 0 blank ground
            "Tiles/tile_0012", // 1 ground v2
            "Tiles/tile_0024", // 2 ground v3
            "Tiles/tile_0005", // 3 right upper corner
            "Tiles/tile_0017", // 4 right bottom corner
            "Tiles/tile_0016", // 5 left bottom corner
            "Tiles/tile_0004", // 6 left upper corner
            "Tiles/tile_0015", // 7 top left wall
            "Tiles/tile_0013", // 8 top right wall
            "Tiles/tile_0002", // 9 top bottom wall
            "Tiles/tile_0026", // 10 top upper wall
            "Tiles/tile_0025", // 11 top bottom left wall edge
            "Tiles/tile_0027", // 12 top bottom right wall edge
            "Tiles/tile_0001", // 13 top upper left wall edge
            "Tiles/tile_0003", // 14 top upper right wall edge
            "Tiles/tile_0089", // 15 chest
            "Tiles/tile_0021", // 16 open doors
            "Tiles/tile_0057", // 17 front left wall edge
            "Tiles/tile_0059", // 18 front left wall edge
            "Tiles/tile_0048", // 19 sand
            "Tiles/tile_0049", // 20 sand with rocks
            "Tiles/tile_0050", // 21 sand with shade on top
            "Tiles/tile_0051", // 22 sand with rocks and shade on top
            "Tiles/tile_0052", // 23 sand with shade on top and right side
            "Tiles/tile_1050", // 24 sand with shade to the left (custom)
            "Tiles/tile_2050", // 25 sand with shade to the right (custom)
            "Tiles/tile_3050", // 26 sand with shade down (custom)
            "Tiles/tile_1052", // 27 sand with shade left up (custom)
            "Tiles/tile_2052", // 28 sand with shade right down (custom)
            "Tiles/tile_3052", // 29 sand with shade left down (custom)
        };

        public static readonly string[] ENEMY_TILE_PATHS =
        {
            "Tiles/tile_0120", // 0. Bat
            "Tiles/tile_0122", // 1. Spider
            "Tiles/tile_0123", // 2. Brown Rat
            "Tiles/tile_0124", // 3. Gray Rat
            "Tiles/tile_0109", // 4. Cyclops
            "Tiles/tile_0110", // 5. Crab
            "Tiles/tile_0111", // 6. Dark Wizard
            "Tiles/tile_0121", // 7. Ghost
        };

        public static readonly int[][] ENEMY_SPAWN_PATTERN =
        {
            new int[] { 0, 1, 2, 3 },
            new int[] { 4, 5 },
            new int[] { 6, 7 }
        };

        public const string PLAYER_TILE_PATH = "Tiles/tile_0096";
        public static readonly string[] HUD_TILE_PATHS =
        {
            "Tiles/tile_0113", // 0. Empty potion
            "Tiles/tile_0114", // 1. Green potion
            "Tiles/tile_0115", // 2. Red potion
            "Tiles/tile_0116", // 3. Blue potion
        };

        // Original tile pattern TPO = Tile patter Open
        // Set of Rooms for first main map
        public static readonly int[,] TPO_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 17, 16, 18, 3 },
            { 7, 2, 1, 0, 0, 11 },
            { 7, 0, 0, 0, 0, 0 },
            { 7, 0, 0, 1, 0, 13 },
            { 7, 2, 0, 0, 15, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 0, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 0, 0, 0, 8 },
            { 7, 2, 0, 0, 2, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 0, 11 },
            { 0, 0, 0, 0, 0, 0 },
            { 14, 0, 0, 0, 0, 13 },
            { 7, 1, 0, 0, 0, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_LEFT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 2, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 1, 1, 0, 8 },
            { 7, 15, 0, 0, 0, 8 },
            { 5, 9, 14, 0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_LEFT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 12, 0, 11, 3 },
            { 12, 0, 0, 0, 0, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 1, 1, 0, 8 },
            { 7, 2, 0, 2, 2, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_DOWN = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7, 0, 1, 0, 0, 11 },
            { 7, 1, 0, 0, 0, 0 },
            { 7, 0, 0, 0, 0, 13 },
            { 7, 0, 0, 0, 0, 8 },
            { 5, 9, 14, 0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_RIGHT = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 6, 10, 12, 0, 11, 3 },
            { 7, 2, 0, 0, 0, 11 },
            { 7, 15, 0, 0, 2, 0 },
            { 7, 0, 1, 0, 0, 13 },
            { 7, 2, 0, 0, 0, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };

        public static readonly int[,][,] MAP_ROOM_PATTERN = new int[ROW_COUNT, COLUMN_COUNT][,]
        {
            {
                TPO_RIGHT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_LEFT_DOWN
            },
            {
                TPO_RIGHT_DOWN,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_UP_LEFT
            },
            {
                TPO_UP_RIGHT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_RIGHT_LEFT,
                TPO_LEFT
            }
        };

        // Set of Rooms for first encounter map (open rooms) FER = First Encounter Room LU = Left Upper
        public static readonly int[,] FER_LU = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 27, 21, 21, 21, 21, 21 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 20, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_RU = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 21, 21, 21, 21, 22, 23 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 20, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 }
        };
        public static readonly int[,] FER_U = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 21, 21, 21, 21, 21, 21 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 20, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_C = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 20, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 20, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_LC = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 20, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_RC = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 20, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 }
        };
        public static readonly int[,] FER_D = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 20, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 26, 26, 26, 26, 26, 26 }
        };
        public static readonly int[,] FER_LD = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 20, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 29, 26, 26, 26, 26, 26 }
        };
        public static readonly int[,] FER_RD = new int[ROOM_SIZE, ROOM_SIZE]
        {
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 20, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 26, 26, 26, 26, 26, 28 }
        };

        public static readonly int[,][,] MAP_ROOM_PATTERN_FIRST_ENCOUNTER = new int[
            ROW_COUNT,
            COLUMN_COUNT
        ][,]
        {
            { FER_LU, FER_U, FER_U, FER_U, FER_U, FER_RU },
            { FER_LC, FER_C, FER_C, FER_C, FER_C, FER_RC },
            { FER_LD, FER_D, FER_D, FER_D, FER_D, FER_RD }
        };
    }
}
