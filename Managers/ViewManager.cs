using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Data;
using SlipThrough2.Views;

namespace SlipThrough2.Managers
{
    public class ViewManager
    {
        private static string currentView;
        private static readonly Dictionary<string, View> views = new();
        private static readonly Dictionary<Keys, bool> keyStates = new();

        private static (
            Texture2D PlayerTexture,
            List<Texture2D> EnemyTextures,
            List<Texture2D> MapTextures,
            List<Texture2D> HUDTextures,
            SpriteFont Font,
            SpriteBatch spriteBatch
        ) gameAssetsBackup;

        private static ViewsStructures viewsData;

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
            viewsData = DataStructure._constants.Views;

            views[viewsData.StartScreen.Name] = new Start(viewsData.StartScreen.Name);
            views[viewsData.MainGame.Name] = new MainGame(viewsData.MainGame.Name, gameAssets);
            views[viewsData.Options.Name] = new Options(viewsData.Options.Name);

            View.font = gameAssets.Font;
            View.spriteBatch = gameAssets.spriteBatch;
            currentView = viewsData.StartScreen.Name;
            gameAssetsBackup = gameAssets;
        }

        public static void Update(Game1 game1)
        {
            HandleKeyPresses(game1);

            // Update the current view
            views[currentView].Update();
        }

        public static void Draw()
        {
            views[currentView].Draw();
        }

        public static void SwitchView(string newName)
        {
            currentView = newName;
        }

        private static void HandleKeyPresses(Game1 game1)
        {
            List<TrackedKey> trackedKeys = DataStructure._constants.Settings.TrackedKeys;
            foreach (var trackedKey in trackedKeys)
            {
                Keys key = (Keys)trackedKey.Value;
                // Check if the key is pressed and was not previously pressed
                if (Keyboard.GetState().IsKeyDown(key) && !keyStates[key])
                {
                    keyStates[key] = true; // Mark the value as being pressed

                    // Use switch statement for key-specific actions
                    switch (key)
                    {
                        case Keys.Escape:
                            // Options / Quit
                            if (currentView != viewsData.Options.Name)
                                SwitchView(viewsData.Options.Name);
                            else
                                game1.Exit();
                            break;

                        case Keys.Enter:
                            // Start / Return
                            SwitchView(viewsData.MainGame.Name);
                            break;

                        case Keys.R:
                            // Reset (only if in Options)
                            if (currentView == viewsData.Options.Name)
                            {
                                // Remove the most significant removable element of this view - remove all enemies
                                views[viewsData.MainGame.Name].Remove();

                                // Reconstruct entities and maps
                                views[viewsData.MainGame.Name] = new MainGame(
                                    viewsData.MainGame.Name,
                                    gameAssetsBackup
                                );
                                SwitchView(viewsData.MainGame.Name);
                            }
                            break;
                    }
                }

                // Check if the key is released and mark it as not pressed
                if (Keyboard.GetState().IsKeyUp(key))
                    keyStates[key] = false;
            }
        }
    }
}
