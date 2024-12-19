using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class StartViewBackground
    {
        // The texture to draw as a background.
        private Texture2D background;
        private static Settings data = DataStructure._constants.Settings;
        public static float scrollingSpeed = 100;
        private Vector2 position;
        private float scale;

        public StartViewBackground(Texture2D Background)
        {
            background = Background;

            // To fit the background to the window
            float scaleY = (float)data.WindowHeight / background.Height;
            float scaleX = (float)data.WindowWidth / background.Width;
            
            // Use the larger scale
            scale = scaleX > scaleY ? scaleX : scaleY;
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
            batch.Draw(texture: background, new Rectangle(x, y, width, height), Color.White);

            // Second beginning from below, outside of the window and going up, into the window
            batch.Draw(
                texture: background,
                new Rectangle(x, y - height, width, height),
                Color.White
            );
        }
    }
}
