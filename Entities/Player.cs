using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using static SlipThrough2.Constants;


namespace SlipThrough2.Entities
{
    public class Player
    {
        private Vector2 position;
        private Texture2D texture;
        private int iteration;

        public Player(Texture2D playerTexture)
        {
            texture = playerTexture;
            position = new Vector2(CELL_SIZE * 1, CELL_SIZE * 1); // Starting position, for example
        }

        public void Update(GameTime gameTime)
        {
            iteration++;

            if (iteration % ITERATION_TIME != 0) return;

            // Check if the new move would not be entering a wall
            KeyboardState state = Keyboard.GetState();
            int velocity = CELL_SIZE;
            if (state.IsKeyDown(Keys.W) && moveIsLegal('W')) position.Y -= velocity;
            if (state.IsKeyDown(Keys.S) && moveIsLegal('S')) position.Y += velocity;
            if (state.IsKeyDown(Keys.A) && moveIsLegal('A')) position.X -= velocity;
            if (state.IsKeyDown(Keys.D) && moveIsLegal('D')) position.X += velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    CELL_SIZE,
                    CELL_SIZE
                ),
                Color.Gray
            );
        }

        // Helper functions
        public bool moveIsLegal(char direction)
        {
            Debug.WriteLine(position);
            if (direction == 'W')
            {
                // Always the top most edge
                if (position.Y == CELL_SIZE) return false;

                // If the cell in the global grid above the player is a wall

                // TODO: make a data structure for where all the other "custom" walls are and detect them here (check just the neighbors)
            }

            if (direction == 'S')
            {
                // Always the top most edge
                if (position.Y == WINDOW_HEIGHT - 2 * CELL_SIZE) return false;
            }

            if (direction == 'A')
            {
                // Always the top most edge
                if (position.X == CELL_SIZE) return false;
            }

            if (direction == 'D')
            {
                // Always the top most edge
                if (position.X == WINDOW_WIDTH - 2 * CELL_SIZE) return false;
            }
            return true;
        }
    }
}
