using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SlipThrough2.World
{
    public class Map
    {
        private List<Texture2D> textures;

        public Map(List<Texture2D> mapTextures)
        {
            textures = mapTextures;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Map is divided into a grid of rooms
            // Each room is divided into a grid of tiles
            // Draw each tile in a horizontal line of the room, then
            // Draw each horizontal line of the room, then
            // Draw each room in a horizontal line of the map, then
            // Draw each horizontal line of the grid in the map
            for (int column = 0; column < Constants.COLUMN_COUNT; column++)
            {
                for (int row = 0; row < Constants.ROW_COUNT; row++)
                {
                    for (int y = 0; y < Constants.ROOM_SIZE; y++)
                    {
                        for (int x = 0; x < Constants.ROOM_SIZE; x++)
                        {
                            int tileId = Constants.TILE_PATTERNS[column + Constants.COLUMN_COUNT * row][y, x];
                            Texture2D tileTexture = textures[tileId];
                            spriteBatch.Draw(
                                tileTexture,
                                new Rectangle(
                                    x * Constants.CELL_SIZE + column * Constants.ROOM_SIZE * Constants.CELL_SIZE,
                                    y * Constants.CELL_SIZE + row * Constants.ROOM_SIZE * Constants.CELL_SIZE,
                                    Constants.CELL_SIZE,
                                    Constants.CELL_SIZE
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
