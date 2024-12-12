using Microsoft.Xna.Framework;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class Start : View
    {
        private static Settings data = DataStructure._constants.Settings;

        public Start(string viewName) => view = viewName;

        public override void Update() { }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            DisplayText(view, new Vector2(data.WindowWidth, data.WindowHeight), "right");

            // Display large game name
            string[] textToDisplay =
            {
                "Slip Through 2",
                "", // Just for spacing
                "- Press Enter key to start",
                "- Press Esc to go to Options"
            };

            DisplayLinesOfText(textToDisplay);
        }

        public override void Remove() { }
    }
}
