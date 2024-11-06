using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            position = new Vector2(Constants.CELL_SIZE * 1, Constants.CELL_SIZE * 1); // Starting position, for example
        }

        public void Update(GameTime gameTime)
        {
            iteration++;

            if (iteration % 60 != 0) return;

            // Example of movement
            KeyboardState state = Keyboard.GetState();
            int velocity = Constants.CELL_SIZE;
            if (state.IsKeyDown(Keys.W)) position.Y -= velocity;
            if (state.IsKeyDown(Keys.S)) position.Y += velocity;
            if (state.IsKeyDown(Keys.A)) position.X -= velocity;
            if (state.IsKeyDown(Keys.D)) position.X += velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Constants.CELL_SIZE, Constants.CELL_SIZE), Color.Gray);
        }
    }
}
