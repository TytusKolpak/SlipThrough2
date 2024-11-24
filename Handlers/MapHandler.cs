using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Handlers
{
    public class MapHandler
    {
        private readonly List<Texture2D> textures;

        // Of rooms in a map (and of tiles in a room)
        public static int[,][,] currentPattern;

        // Where player can move, what to enter and so on
        public static int[,] currentFunctionalPattern;

        // For easy tracking
        public static MAP_NAME mapName;

        public MapHandler(List<Texture2D> mapTextures)
        {
            textures = mapTextures;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Map is divided into a grid of rooms
            // Each room is divided into a grid of tiles
            // Draw each tile in a horizontal line of the room, then
            // Draw each horizontal line of the room, then
            // Draw each room in a horizontal line of the map, then
            // Draw each horizontal line of the grid in the map
            for (int column = 0; column < COLUMN_COUNT; column++)
            {
                for (int row = 0; row < ROW_COUNT; row++)
                {
                    for (int y = 0; y < ROOM_SIZE; y++)
                    {
                        for (int x = 0; x < ROOM_SIZE; x++)
                        {
                            int tileId = currentPattern[row, column][y, x];
                            Texture2D tileTexture = textures[tileId];
                            spriteBatch.Draw(
                                tileTexture,
                                new Rectangle(
                                    x * CELL_SIZE + column * ROOM_SIZE * CELL_SIZE,
                                    y * CELL_SIZE + row * ROOM_SIZE * CELL_SIZE,
                                    CELL_SIZE,
                                    CELL_SIZE
                                ),
                                Color.White
                            );
                        }
                    }
                }
            }
        }
    }
}
