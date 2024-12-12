using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public abstract class View
    {
        public string view;
        public static SpriteFont font;
        public static SpriteBatch spriteBatch;
        private static readonly int windowWidth = DataStructure._constants.Settings.WindowWidth,
            windowHeight = DataStructure._constants.Settings.WindowHeight,
            fontSize = DataStructure._constants.Settings.FontSize;

        public abstract void Update();

        public abstract void Draw();

        public static void DisplayText(string text, Vector2 position, string justify)
        {
            Vector2 origin = justify switch
            {
                "left" => Vector2.Zero,
                "center" => font.MeasureString(text) / 2,
                _ => font.MeasureString(text)
            };

            spriteBatch.DrawString(font, text, position, Color.Black, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
        }

        public static void DisplayLinesOfText(string[] textToDisplay){
            
            float verticalCenter = windowHeight / 2;
            float offsetToTop = -fontSize * (textToDisplay.Length - 1);

            for (int i = 0; i < textToDisplay.Length; i++)
            {
                float x = windowWidth * 0.5f;
                float y = verticalCenter + offsetToTop + i * 2 * fontSize;

                DisplayText(textToDisplay[i], new Vector2(x, y), "center");
            }
        }
        public abstract void Remove();
    }
}
