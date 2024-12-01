using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;

namespace SlipThrough2.Entities
{
    // Player is a player and a playerManager at the same time
    public class Player : Entity
    {
        public static bool[] availableMoves;
        private static Settings settingsData;

        public Player(Texture2D playerTexture)
        {
            settingsData = DataStructure._constants.Settings;
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

            if (!entityIsCooledDown)
            {
                HandleCooldown();
                return;
            }

            KeyManager.HandlePlayerMovement(this);
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

        public void ApplyMovement(Vector2 direction)
        {
            entityIsCooledDown = false;
            position += direction;
        }
    }
}
