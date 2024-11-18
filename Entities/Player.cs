using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    // Player is a player and a playerManager at the same time
    public class Player : Entity
    {
        public bool[] availableMoves = { true, true, true, true };

        public Player(Texture2D playerTexture)
        {
            texture = playerTexture;
            // Starting position is cell: (1,1) for example
            position = new Vector2(CELL_SIZE * 1, CELL_SIZE * 1);

            // -1 just means "for the player"
            AssignStats(-1);
        }

        public void Update()
        {
            if (!entityIsCooledDown)
            {
                HandleCooldown();
                return;
            }

            // Define movement directions and their corresponding keys and move validation
            var movements = new (Keys key, int moveIndex, Vector2 direction)[]
            {
                (Keys.W, 0, new Vector2(0, -CELL_SIZE)), // Up
                (Keys.D, 1, new Vector2(CELL_SIZE, 0)), // Right
                (Keys.S, 2, new Vector2(0, CELL_SIZE)), // Down
                (Keys.A, 3, new Vector2(-CELL_SIZE, 0)) // Left
            };

            KeyboardState state = Keyboard.GetState();

            foreach (var (key, moveIndex, direction) in movements)
            {
                if (state.IsKeyDown(key) && availableMoves[moveIndex])
                {
                    ApplyMovement(direction);
                    return;
                    // Prevents diagonal movement
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, CELL_SIZE, CELL_SIZE),
                Color.White
            );
        }

        private void ApplyMovement(Vector2 direction)
        {
            entityIsCooledDown = false;
            position += direction;
        }
    }
}
