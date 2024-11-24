using System.Collections.Generic;

namespace SlipThrough2
{
    public static class Constants
    {
        public static readonly int[][] ENEMY_SPAWN_PATTERN =
        {
            new int[] { 0 }, //, 1, 2, 3 }, // for easier testing
            new int[] { 4, 5 },
            new int[] { 6, 7 }
        };

        public static readonly Dictionary<string, Dictionary<string, int>> STATS =
            new()
            {
                {
                    "Player",
                    new Dictionary<string, int>
                    {
                        { "maxHealth", 5 },
                        { "health", 4 },
                        { "maxMana", 3 },
                        { "mana", 1 },
                        { "attack", 1 },
                    }
                },
                {
                    "EasyEnemy",
                    new Dictionary<string, int>
                    {
                        { "maxHealth", 2 },
                        { "health", 2 },
                        { "maxMana", 1 },
                        { "mana", 1 },
                        { "attack", 1 },
                    }
                },
                {
                    "MediumEnemy",
                    new Dictionary<string, int>
                    {
                        { "maxHealth", 5 },
                        { "health", 5 },
                        { "maxMana", 2 },
                        { "mana", 2 },
                        { "attack", 2 },
                    }
                },
                {
                    "HardEnemy",
                    new Dictionary<string, int>
                    {
                        { "maxHealth", 15 },
                        { "health", 15 },
                        { "maxMana", 5 },
                        { "mana", 5 },
                        { "attack", 3 },
                    }
                }
            };

        // Original tile pattern TPO = Tile patter Open
        // Set of Rooms for first main map
        public static readonly int[,] TPO_RIGHT = new int[6, 6]
        {
            { 6, 10, 17, 16, 18, 3 },
            { 7, 2, 1, 0, 0, 11 },
            { 7, 0, 0, 0, 0, 0 },
            { 7, 0, 0, 1, 0, 13 },
            { 7, 2, 0, 0, 15, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_LEFT = new int[6, 6]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 0, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 0, 0, 0, 8 },
            { 7, 2, 0, 0, 2, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_LEFT = new int[6, 6]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 0, 11 },
            { 0, 0, 0, 0, 0, 0 },
            { 14, 0, 0, 0, 0, 13 },
            { 7, 1, 0, 0, 0, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_LEFT_DOWN = new int[6, 6]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 12, 0, 1, 0, 2, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 1, 1, 0, 8 },
            { 7, 15, 0, 0, 0, 8 },
            { 5, 9, 14, 0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_LEFT = new int[6, 6]
        {
            { 6, 10, 12, 0, 11, 3 },
            { 12, 0, 0, 0, 0, 8 },
            { 0, 0, 0, 0, 2, 8 },
            { 14, 0, 1, 1, 0, 8 },
            { 7, 2, 0, 2, 2, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };
        public static readonly int[,] TPO_RIGHT_DOWN = new int[6, 6]
        {
            { 6, 10, 10, 10, 10, 3 },
            { 7, 0, 1, 0, 0, 11 },
            { 7, 1, 0, 0, 0, 0 },
            { 7, 0, 0, 0, 0, 13 },
            { 7, 0, 0, 0, 0, 8 },
            { 5, 9, 14, 0, 13, 4 }
        };
        public static readonly int[,] TPO_UP_RIGHT = new int[6, 6]
        {
            { 6, 10, 12, 0, 11, 3 },
            { 7, 2, 0, 0, 0, 11 },
            { 7, 15, 0, 0, 2, 0 },
            { 7, 0, 1, 0, 0, 13 },
            { 7, 2, 0, 0, 0, 8 },
            { 5, 9, 9, 9, 9, 4 }
        };

        public static readonly int[,][,] MAP_ROOM_PATTERN = new int[3, 6][,]
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
        public static readonly int[,] FER_LU = new int[6, 6]
        {
            { 27, 21, 21, 21, 21, 21 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 20, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_RU = new int[6, 6]
        {
            { 21, 21, 21, 21, 22, 23 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 20, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 }
        };
        public static readonly int[,] FER_U = new int[6, 6]
        {
            { 21, 21, 21, 21, 21, 21 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 20, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_C = new int[6, 6]
        {
            { 20, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 20, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_LC = new int[6, 6]
        {
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 20, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 }
        };
        public static readonly int[,] FER_RC = new int[6, 6]
        {
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 20, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 }
        };
        public static readonly int[,] FER_D = new int[6, 6]
        {
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 19, 20, 19, 19, 19, 19 },
            { 19, 19, 19, 19, 19, 19 },
            { 26, 26, 26, 26, 26, 26 }
        };
        public static readonly int[,] FER_LD = new int[6, 6]
        {
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 24, 20, 19, 19, 19, 19 },
            { 24, 19, 19, 19, 19, 19 },
            { 29, 26, 26, 26, 26, 26 }
        };
        public static readonly int[,] FER_RD = new int[6, 6]
        {
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 19, 20, 19, 19, 19, 25 },
            { 19, 19, 19, 19, 19, 25 },
            { 26, 26, 26, 26, 26, 28 }
        };

        public static readonly int[,][,] MAP_ROOM_PATTERN_FIRST_ENCOUNTER = new int[3, 6][,]
        {
            { FER_LU, FER_U, FER_U, FER_U, FER_U, FER_RU },
            { FER_LC, FER_C, FER_C, FER_C, FER_C, FER_RC },
            { FER_LD, FER_D, FER_D, FER_D, FER_D, FER_RD }
        };
    }
}
