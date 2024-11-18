using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public class Enemy : Entity
    {
        public Enemy(Texture2D enemyTexture, int doorNumber)
        {
            texture = enemyTexture;
            position = new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2); // Starting position, for example
            AssignStats(doorNumber);
        }

        public void Update()
        {
            if (!entityIsCooledDown)
            {
                HandleCooldown();
                return;
            }

            entityIsCooledDown = false;

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
