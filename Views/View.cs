using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlipThrough2.Views
{
    public abstract class View
    {
        public string view;
        public static SpriteFont font;
        public static SpriteBatch spriteBatch;

        public abstract void Update();

        public abstract void Draw();

        public static void DisplayText(string text, Vector2 position, int textAlignmentType)
        {
            Vector2 origin = textAlignmentType switch
            {
                0 => Vector2.Zero,
                1 => font.MeasureString(text) / 2,
                _ => font.MeasureString(text)
            };

            spriteBatch.DrawString(
                font,
                text,
                position,
                Color.Black,
                0,
                origin,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
        }

        public abstract void Remove();
    }
}
