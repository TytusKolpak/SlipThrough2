using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;

namespace SlipThrough2.Entities
{
    // Player is a player and a playerManager at the same time
    public class Player : Entity
    {
        public static bool[] availableMoves;

        public Player(Texture2D playerTexture)
        {
            texture = playerTexture;
            // Starting position is cell: (1,1) for example
            position = new Vector2(settingsData.CellSize * 1, settingsData.CellSize * 1);

            // -1 just means "for the player"
            AssignStats(-1);
        }

        public void Update()
        {
            int PCX = (int)position.X / settingsData.CellSize;
            int PCY = (int)position.Y / settingsData.CellSize;

            int[,] pattern = MapHandler.currentFunctionalPattern;

            bool IsValid(int y, int x)
            {
                // Player is within bounds (don't check outside the map)
                if (y < 0 || y >= settingsData.MapHeight || x < 0 || x >= settingsData.MapWidth)
                    return false;

                // Player wants to eneter a terrain cell (1) or door cell (-1, 1 and above)
                // Walls are 0
                return pattern[y, x] != 0;
            }

            // Define movement validity checks (order is important)
            availableMoves = new bool[]
            {
                IsValid(PCY - 1, PCX + 1), // Right Up
                IsValid(PCY + 1, PCX + 1), // Right Down
                IsValid(PCY + 1, PCX - 1), // Left Down
                IsValid(PCY - 1, PCX - 1), // Left Up
                IsValid(PCY - 1, PCX), // Up
                IsValid(PCY, PCX + 1), // Right
                IsValid(PCY + 1, PCX), // Down
                IsValid(PCY, PCX - 1), // Left
            };

            /* I think we would need to affect iteration time in some way
            than with such calculations for setting speed of movement
            bc it should determine in what amount of time an entity will shift 32 units (1 cell),
            and not how much units it will shift in set amount of iterations (of time).
            This would keep total shift uniform, but time would be modifyable
            Also this approach accumulates rounding errors. It is generally insignificant
            between cells, but at the end of a movement this value needs to be round so either
            constatnt correcting is to be applied or let it be ignored and round it at the end*/

            position += direction * speed / cellSize / modifier;

            if (!entityIsCooledDown)
            {
                HandleCooldown();
                return;
            }

            KeyManager.SetPlayerDirection(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int heightInStep = timeForShift / 2 - Math.Abs(idleIterations - timeForShift / 2);

            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y - heightInStep,
                    settingsData.CellSize,
                    settingsData.CellSize
                ),
                Color.White
            );
        }
    }
}
