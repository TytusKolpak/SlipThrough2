using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Views;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class ViewManager
    {
        private static VIEW_NAME currentView;
        private Dictionary<VIEW_NAME, View> views = new();
        private Dictionary<Keys, bool> keyStates = new();

        private (
            Texture2D PlayerTexture,
            List<Texture2D> EnemyTextures,
            List<Texture2D> MapTextures,
            List<Texture2D> HUDTextures,
            SpriteFont Font,
            SpriteBatch spriteBatch
        ) gameAssetsBackup;

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
            gameAssetsBackup = gameAssets;

            // Populate the dictionary with default states (all keys are initially not pressed)
            foreach (var key in TRACKED_KEYS)
            {
                keyStates[key] = false; // False means key is not pressed
            }
        }

        public static void SwitchView(VIEW_NAME newName)
        {
            currentView = newName;
        }

        public void Update(Game1 game1)
        {
            HandleKeyPresses(game1);

            // Update the current view
            views[currentView].Update();
        }

        public void Draw()
        {
            views[currentView].Draw();
        }

        private void HandleKeyPresses(Game1 game1)
        {
            foreach (var key in TRACKED_KEYS)
            {
                // Check if the key is pressed and was not previously pressed
                if (Keyboard.GetState().IsKeyDown(key) && !keyStates[key])
                {
                    keyStates[key] = true; // Mark the key as being pressed
                    Console.WriteLine($"{key} is pressed.");

                    // Use switch statement for key-specific actions
                    switch (key)
                    {
                        case Keys.Escape:
                            // Options / Quit
                            if (currentView != VIEW_NAME.Options)
                                SwitchView(VIEW_NAME.Options);
                            else
                                game1.Exit();
                            break;

                        case Keys.Enter:
                            // Start / Return
                            SwitchView(VIEW_NAME.MainGame);
                            break;

                        case Keys.R:
                            // Reset (only if in Options)
                            if (currentView == VIEW_NAME.Options)
                            {
                                // Remove the most significant removable element of this view - remove all enemies
                                views[VIEW_NAME.MainGame].Remove();

                                // Reconstruct entities and maps
                                views[VIEW_NAME.MainGame] = new MainGame(
                                    VIEW_NAME.MainGame,
                                    gameAssetsBackup
                                );
                                SwitchView(VIEW_NAME.MainGame);
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
