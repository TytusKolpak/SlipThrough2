using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Managers;

namespace SlipThrough2.Views
{
    public class Options : View
    {
        private static readonly Settings data = DataStructure._constants.Settings;
        private MovingBackground myBackground;

        public Options(string viewName, List<Texture2D> BackgroundTextures)
        {
            view = viewName;
            myBackground = new MovingBackground(BackgroundTextures);
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

            string mapTypeText = MapManager.newMappingApplied ? "old" : "new";
            string audioText = AudioManager.enableSoundEffects ? "off" : "on";
            string[] textToDisplay =
            {
                "- Press Q to Quit",
                "- Press Esc key to Resume",
                "- Press R key to Restart",
                $"- Press S key to Switch to {mapTypeText} main map",
                $"- Press E to turn sound effects {audioText}",
            };

            DisplayLinesOfText(textToDisplay, Color.White);
            myBackground.Draw(spriteBatch);
        }

        public override void Remove() { }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myBackground.Update(elapsed * MovingBackground.scrollingSpeed);
        }
    }
}
