using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            position = new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2); // Starting position, for example
            iteration = 0;
        }

        public void Update()
        {
            iteration++;

            if (iteration % ITERATION_TIME != 0)
                return;

            Random rnd = new();
            position.X += CELL_SIZE * (rnd.Next(3) - 1);
            position.Y += CELL_SIZE * (rnd.Next(3) - 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, CELL_SIZE, CELL_SIZE),
                Color.White
            );
        }
    }
}
