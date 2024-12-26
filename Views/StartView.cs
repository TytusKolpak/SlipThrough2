using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class Start : View
    {
        private static Settings data = DataStructure._constants.Settings;
        private MovingBackground myBackground;

        public Start(string viewName, Texture2D Background)
        {
            view = viewName;
            myBackground = new MovingBackground(Background);
        }

        public override void Update(GameTime gameTime)
        {
            // The time since Update was called last.
            // Usually it's 0.01(6)s but it is not guaranteed to stay like that when load increases
            // So to mimic normal behavior we will move the background more if more time passed
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            myBackground.Update(elapsed * MovingBackground.scrollingSpeed);
        }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            DisplayText(
                view,
                new Vector2(data.WindowWidth, data.WindowHeight),
                "right",
                Color.White
            );

            // Display large game name
            string[] textToDisplay =
            {
                "Slip Through 2",
                "", // Just for spacing
                "- Press Enter key to start",
                "- Press Esc to go to Options"
            };

            DisplayLinesOfText(textToDisplay, Color.White);

            myBackground.Draw(spriteBatch);
        }

        public override void Remove() { }
    }
}
