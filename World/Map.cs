using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SlipThrough2.World
{

    public class Map
    {
        private int[,] mapPattern = {
            { 6, 10, 10, 10, 10, 3 },
            { 7,  0,  1,  0,  0, 8 },
            { 7,  0,  0,  0,  2, 8 },
            { 7,  0,  1,  1,  0, 8 },
            { 7,  2,  0,  0,  0, 8 },
            { 5,  9,  9,  9,  9, 4 }
        };

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
            for (int y = 0; y < mapPattern.GetLength(0); y++)
            {
                for (int x = 0; x < mapPattern.GetLength(1); x++)
                {
                    int tileId = mapPattern[y, x];
                    Texture2D tileTexture = textures[tileId];
                    spriteBatch.Draw(tileTexture, new Rectangle(x * Constants.CELL_SIZE, y * Constants.CELL_SIZE, Constants.CELL_SIZE, Constants.CELL_SIZE), Color.White);
                }
            }

        }
    }
}
