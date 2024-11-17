using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public class Player
    {
        private readonly Texture2D texture;
        private bool playerIsCooledDown = true;
        private int idleIterations = 0;
        public Vector2 position;
        public HUD HUD;
        public bool HUDIsVisible = false;
        public bool[] availableMoves = { true, true, true, true };

        public Player(Texture2D playerTexture, List<Texture2D> HUDTextures, SpriteFont font)
        {
            texture = playerTexture;
            // Starting position is cell: (1,1) for example
            position = new Vector2(CELL_SIZE * 1, CELL_SIZE * 1);
            HUD = new HUD(HUDTextures, font);
        }

        public void Update(MAP_NAME mapName)
        {
            if (mapName != MAP_NAME.Main)
            {
                HUDIsVisible = true;
            }
            else
            {
                HUDIsVisible = false;
            }

            if (!playerIsCooledDown)
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

        public void Draw(SpriteBatch spriteBatch, MAP_NAME mapName)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, CELL_SIZE, CELL_SIZE),
                Color.White
            );

            if (HUDIsVisible)
            {
                HUD.Draw(spriteBatch);
            }
        }

        private void HandleCooldown()
        {
            idleIterations++;
            if (idleIterations > ITERATION_TIME)
            {
                idleIterations = 0;
                playerIsCooledDown = true;
            }
        }

        private void ApplyMovement(Vector2 direction)
        {
            playerIsCooledDown = false;
            position += direction;
        }
    }
}
