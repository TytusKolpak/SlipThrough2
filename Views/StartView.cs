using Microsoft.Xna.Framework;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class Start : View
    {
        private static Settings settingsData;

        public Start(string viewName)
        {
            settingsData = DataStructure._constants.Settings;
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

            // Display large game name
            textToDisplay = "Slip Through 2";
            DisplayText(
                textToDisplay,
                new Vector2(
                    settingsData.WindowWidth * 0.5f,
                    settingsData.WindowHeight * 0.3f + settingsData.FontSize
                ),
                1
            );

            textToDisplay = "Press Enter key to start";
            DisplayText(
                textToDisplay,
                new Vector2(
                    settingsData.WindowWidth * 0.5f,
                    settingsData.WindowHeight * 0.3f + settingsData.FontSize * 5
                ),
                1
            );

            textToDisplay = "Press Esc to go to Options";
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
