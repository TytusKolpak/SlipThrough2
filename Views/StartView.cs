using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public class Start : View
    {
        public Start(VIEW_NAME viewName)
        {
            view = viewName;
        }

        public override void Update() { }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            string textToDisplay = view.ToString();
            DisplayText(textToDisplay, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), 2);

            // Display large game name
            textToDisplay = "Slip Through 2";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE),
                1
            );

            // Inform that the user can simply press Enter key to start
            // To be implemented
            textToDisplay = "Press Enter key to start";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE * 5),
                1
            );

            // Inform that the user can press Esc key to quit
            // (later to enter Options sreen) to be implemented
            textToDisplay = "Press Esc to quit at any time";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE * 7),
                1
            );
        }
    }
}
