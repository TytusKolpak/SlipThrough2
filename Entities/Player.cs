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
        public static bool[] availableMoves = { true, true, true, true };
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
            // Top, right, down, left
            // (PCY > 0) is there for the reason that when the player is at index 0
            // (upper most cell) then we can't check cell at X=-1 since it doesn't exist
            // The >= 1  check is for 1 representing a free space and
            // 2,3,4,... for doors (each door has it's unique number)
            // Player cell position in grid
            int PCX = (int)position.X / settingsData.CellSize;
            int PCY = (int)position.Y / settingsData.CellSize;

            availableMoves = new bool[4]
            {
                (PCY > 0)
                    && (
                        MapHandler.currentFunctionalPattern[PCY - 1, PCX] >= 1
                        || MapHandler.currentFunctionalPattern[PCY - 1, PCX] == -1
                    ),
                (PCX < settingsData.MapWidth - 1)
                    && MapHandler.currentFunctionalPattern[PCY, PCX + 1] == 1,
                (PCY < settingsData.MapHeight - 1)
                    && MapHandler.currentFunctionalPattern[PCY + 1, PCX] == 1,
                (PCX > 0) && MapHandler.currentFunctionalPattern[PCY, PCX - 1] == 1
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
