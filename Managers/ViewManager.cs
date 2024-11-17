using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Views;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class ViewManager
    {
        private static VIEW_NAME currentView = VIEW_NAME.StartScreen;
        private Dictionary<VIEW_NAME, View> views = new Dictionary<VIEW_NAME, View>();

        public ViewManager(
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D>,
                SpriteFont Font
            ) gameAssets
        )
        {
            views[VIEW_NAME.StartScreen] = new Start(VIEW_NAME.StartScreen);
            views[VIEW_NAME.MainGame] = new MainGame(VIEW_NAME.MainGame, gameAssets);
            views[VIEW_NAME.Options] = new Options(VIEW_NAME.Options);

            currentView = VIEW_NAME.MainGame;
        }

        public void SwitchState(VIEW_NAME newName)
        {
            currentView = newName;
        }

        public void Update()
        {
            views[currentView].Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            views[currentView].Draw(spriteBatch);
        }
    }
}
