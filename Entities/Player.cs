using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using static SlipThrough2.Constants;


namespace SlipThrough2.Entities
{
    public class Player
    {
        private Texture2D texture;
        private int iteration;

        // Can go top, right, bottom, left (as in clockwise)
        // Controlled by Map.cs
        public Vector2 position;
        public bool[] availableMoves = new bool[4] { true, true, true, true }; 

        public Player(Texture2D playerTexture)
        {
            texture = playerTexture;
            position = new Vector2(CELL_SIZE * 1, CELL_SIZE * 1); // Starting position is cell: (1,1) for example
        }

        public void Update(GameTime gameTime)
        {
            iteration++;

            if (iteration % ITERATION_TIME != 0) return;

            // Check if the new move would not be entering a wall
            // elses are here so that diagonal movement is impossible
            KeyboardState state = Keyboard.GetState();
            int velocity = CELL_SIZE;
            if (state.IsKeyDown(Keys.W) && availableMoves[0]) position.Y -= velocity;
            else if (state.IsKeyDown(Keys.D) && availableMoves[1]) position.X += velocity;
            else if(state.IsKeyDown(Keys.S) && availableMoves[2]) position.Y += velocity;
            else if(state.IsKeyDown(Keys.A) && availableMoves[3]) position.X -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, CELL_SIZE, CELL_SIZE), Color.Gray);
        }
    }
}
