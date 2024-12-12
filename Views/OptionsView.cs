using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class Options : View
    {
        private static readonly Settings data = DataStructure._constants.Settings;

        public Options(string viewName) => view = viewName;

        public override void Update() { }

        public override void Draw()
        {
            // Bottom right corner says the name of the screen (view)
            DisplayText(view, new Vector2(data.WindowWidth, data.WindowHeight), "right");

            string[] textToDisplay =
            {
                "- Press Esc again to Quit",
                "- Press Enter key to Resume",
                "- Press R key to Restart",
                "- Press S key to Switch between",
                "old and new main map"
            };

            DisplayLinesOfText(textToDisplay);
        }

        public override void Remove() { }
    }
}
