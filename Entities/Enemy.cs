using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlipThrough2.Entities
{
    public class Enemy : Entity
    {
        public Enemy(Texture2D enemyTexture, int doorNumber)
        {
            texture = enemyTexture;
            position = new Vector2(settingsData.WindowWidth / 2, settingsData.WindowHeight / 2); // Starting position, for example
            AssignStats(doorNumber);
        }

        public void Update()
        {
            // Handle movement
            position += direction * speed / cellSize / modifier;

            if (!entityIsCooledDown)
            {
                HandleCooldown();
                return;
            }

            // Handle setting direction
            Random rnd = new();
            direction = new(
                settingsData.CellSize * (rnd.Next(3) - 1),
                settingsData.CellSize * (rnd.Next(3) - 1)
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    settingsData.CellSize,
                    settingsData.CellSize
                ),
                Color.White
            );
        }
    }
}
