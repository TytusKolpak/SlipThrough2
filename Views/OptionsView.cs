using Microsoft.Xna.Framework;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public class Options : View
    {
        public Options(VIEW_NAME viewName)
        {
            view = viewName;
        }

        public override void Update() { }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            string textToDisplay = view.ToString();
            DisplayText(textToDisplay, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), 2);

            textToDisplay = "Press Esc again to Quit";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE * 3),
                1
            );

            textToDisplay = "Press Enter key to Resume";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE * 5),
                1
            );

            textToDisplay = "Press R key to Restart";
            DisplayText(
                textToDisplay,
                new Vector2(WINDOW_WIDTH * 0.5f, WINDOW_HEIGHT * 0.3f + FONT_SIZE * 7),
                1
            );
        }

        public override void Remove() { }
    }
}
