using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public class Options : View
    {
        public Options(VIEW_NAME viewName)
        {
            view = viewName;
        }

        public override void Update()
        {
            TestTK(view);
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
