using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Entities
{
    public class Enemy : Entity
    {
        private static Settings settingsData;

        public Enemy(Texture2D enemyTexture, int doorNumber)
        {
            settingsData = DataStructure._constants.Settings;

            texture = enemyTexture;
            position = new Vector2(settingsData.WindowWidth / 2, settingsData.WindowHeight / 2); // Starting position, for example
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
            Vector2 delta =
                new(
                    settingsData.CellSize * (rnd.Next(3) - 1),
                    settingsData.CellSize * (rnd.Next(3) - 1)
                );
            position += delta;
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
