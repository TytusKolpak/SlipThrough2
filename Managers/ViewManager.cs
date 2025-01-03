using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Views;

namespace SlipThrough2.Managers
{
    public class ViewManager
    {
        public static string currentView;
        public static readonly Dictionary<string, View> views = new();
        public static ViewsStructures viewsData;

        public static (
            Texture2D PlayerTexture,
            List<Texture2D> EnemyTextures,
            List<Texture2D> MapTextures,
            List<Texture2D> HUDTextures,
            List<Texture2D> WeaponTextures,
            SpriteFont Font,
            List<Texture2D> BackgroundTextures,
            SpriteBatch spriteBatch
        ) gameAssetsBackup;

        public static void LoadData(
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                List<Texture2D> WeaponTextures,
                SpriteFont Font,
                List<Texture2D> BackgroundTextures,
                SpriteBatch spriteBatch
            ) gameAssets
        )
        {
            viewsData = DataStructure._constants.Views;

            views[viewsData.StartScreen.Name] = new Start(
                viewsData.StartScreen.Name,
                gameAssets.BackgroundTextures
            );
            views[viewsData.MainGame.Name] = new MainGame(viewsData.MainGame.Name, gameAssets);
            views[viewsData.Options.Name] = new Options(
                viewsData.Options.Name,
                gameAssets.BackgroundTextures
            );

            View.font = gameAssets.Font;
            View.spriteBatch = gameAssets.spriteBatch;
            currentView = viewsData.StartScreen.Name;
            gameAssetsBackup = gameAssets;
        }

        public static void Update(GameTime gameTime)
        {
            views[currentView].Update(gameTime);
        }

        public static void Draw()
        {
            views[currentView].Draw();
        }

        public static void SwitchView(string newName) => currentView = newName;

        public static void ResetViews(string mainView)
        {
            // Remove the most significant removable element of this view - remove all enemies
            views[mainView].Remove();

            // Reconstruct entities and maps
            views[mainView] = new MainGame(mainView, gameAssetsBackup);
            SwitchView(mainView);
        }
    }
}
