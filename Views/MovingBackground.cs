using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class MovingBackground
    {
        // The texture to draw as a background.
        private Texture2D background;
        private static Settings data = DataStructure._constants.Settings;
        public static float scrollingSpeed = 20;
        private Vector2 position;
        private float scale;
        private bool backgroundTooNarrow;

        public MovingBackground(Texture2D Background)
        {
            background = Background;

            // To fit the background to the window
            scale = (float)data.WindowHeight / background.Height;

            // If stretched image is still to narrow then we will need 2 more to the right
            backgroundTooNarrow = background.Width < data.WindowWidth;
        }

        public void Update(float deltaY)
        {
            // Keep moving the position of the background image upwards at all time
            position.Y += deltaY;

            // But when it will be all the way outside of the window, snap it back to fit the view
            if (position.Y > data.WindowHeight)
                position.Y = 0;
        }

        public void Draw(SpriteBatch batch)
        {
            // At all times draw 2 backgrounds

            int x = (int)position.X,
                y = (int)position.Y,
                height = (int)(background.Height * scale),
                width = (int)(background.Width * scale);

            // First beginning from 100% inside of the window and going up, outside of the window
            batch.Draw(background, new Rectangle(x, y, width, height), Color.White); // Main

            // Second beginning from below, outside of the window and going up, into the window
            batch.Draw(background, new Rectangle(x, y - height, width, height), Color.White); // Bottom

            if (backgroundTooNarrow)
            {
                batch.Draw(background, new Rectangle(x + width, y, width, height), Color.White); // Right
                batch.Draw( // Bottom right
                    background,
                    new Rectangle(x + width, y - height, width, height),
                    Color.White
                );
            }
        }
    }
}
