using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Views;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class ViewManager
    {
        private static VIEW_NAME currentView;
        private Dictionary<VIEW_NAME, View> views = new Dictionary<VIEW_NAME, View>();

        public ViewManager(
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                SpriteFont Font,
                SpriteBatch spriteBatch
            ) gameAssets
        )
        {
            views[VIEW_NAME.StartScreen] = new Start(VIEW_NAME.StartScreen);
            views[VIEW_NAME.MainGame] = new MainGame(VIEW_NAME.MainGame, gameAssets);
            views[VIEW_NAME.Options] = new Options(VIEW_NAME.Options);

            View.font = gameAssets.Font;
            View.spriteBatch = gameAssets.spriteBatch;
            currentView = VIEW_NAME.StartScreen;
        }

        public static void SwitchView(VIEW_NAME newName)
        {
            currentView = newName;
        }

        public void Update()
        {
            views[currentView].Update();
        }

        public void Draw()
        {
            views[currentView].Draw();
        }
    }
}
