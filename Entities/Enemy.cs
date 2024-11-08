using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public class Enemy
    {
        private Vector2 position;
        private Texture2D texture;
        private int iteration;

        
        public Enemy(Texture2D enemyTexture)
        {
            texture = enemyTexture;
            position = new Vector2(CELL_SIZE * 3, CELL_SIZE * 2); // Starting position, for example
            iteration = 0;
        }

        public void Update(GameTime gameTime)
        {
            iteration++;

            if (iteration % ITERATION_TIME != 0) return;

            Random rnd = new Random();
            position.X += CELL_SIZE * (rnd.Next(2) - 1);
            position.Y += CELL_SIZE * (rnd.Next(2) - 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, CELL_SIZE, CELL_SIZE), Color.Gray);
        }
    }
}
