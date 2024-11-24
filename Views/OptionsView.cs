using Microsoft.Xna.Framework;
using SlipThrough2.Data;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public class Options : View
    {
        private static Settings settingsData;

        public Options(VIEW_NAME viewName)
        {
            settingsData = ConstantsModel._constants.Settings;

            view = viewName;
        }

        public override void Update() { }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            string textToDisplay = view.ToString();
            DisplayText(
                textToDisplay,
                new Vector2(settingsData.WindowWidth, settingsData.WindowHeight),
                2
            );

            textToDisplay = "Press Esc again to Quit";
            DisplayText(
                textToDisplay,
                new Vector2(
                    settingsData.WindowWidth * 0.5f,
                    settingsData.WindowHeight * 0.3f + settingsData.FontSize * 3
                ),
                1
            );

            textToDisplay = "Press Enter key to Resume";
            DisplayText(
                textToDisplay,
                new Vector2(
                    settingsData.WindowWidth * 0.5f,
                    settingsData.WindowHeight * 0.3f + settingsData.FontSize * 5
                ),
                1
            );

            textToDisplay = "Press R key to Restart";
            DisplayText(
                textToDisplay,
                new Vector2(
                    settingsData.WindowWidth * 0.5f,
                    settingsData.WindowHeight * 0.3f + settingsData.FontSize * 7
                ),
                1
            );
        }

        public override void Remove() { }
    }
}
