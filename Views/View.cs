using System;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public abstract class View
    {
        public VIEW_NAME view;

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

        public void TestTK(VIEW_NAME viewName)
        {
            Console.WriteLine($"This is the {viewName.ToString()} :D!");
        }
    }
}
