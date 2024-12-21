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
            PerformMovement();

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
            entityIsCooledDown = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {   
            spriteBatch.Draw(
                texture: texture,
                destinationRectangle: new Rectangle(
                    x: (int)position.X,
                    y: (int)position.Y,
                    width: settingsData.CellSize,
                    height: settingsData.CellSize
                ),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: new Vector2(0, 0),
                effects: SpriteEffects.None,
                layerDepth: 0.2f // Under everything but the ground which is layer 0 (and weapns on ground)
            );
        }
    }
}
