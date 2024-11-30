using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Views;

namespace SlipThrough2.Managers
{
    public class KeyManager
    {
        private static readonly Dictionary<Keys, bool> keyStates = new();

        public static void Update(Game1 game1) => HandleKeyPresses(game1);

        private static void HandleKeyPresses(Game1 game1)
        {
            List<TrackedKey> trackedKeys = DataStructure._constants.Settings.TrackedKeys;
            string optionsView = ViewManager.viewsData.Options.Name;
            string mainView = ViewManager.viewsData.MainGame.Name;

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
                            if (ViewManager.currentView != optionsView)
                                ViewManager.SwitchView(optionsView);
                            else
                                game1.Exit();
                            break;

                        case Keys.Enter:
                            // Start / Return
                            ViewManager.SwitchView(mainView);
                            break;

                        case Keys.R:
                            // Reset (only if in Options)
                            if (ViewManager.currentView == optionsView)
                            {
                                // Remove the most significant removable element of this view - remove all enemies
                                ViewManager.views[mainView].Remove();

                                // Reconstruct entities and maps
                                ViewManager.views[mainView] = new MainGame(
                                    mainView,
                                    ViewManager.gameAssetsBackup
                                );
                                ViewManager.SwitchView(mainView);
                            }
                            break;
                    }
                }

                // Check if the key is released and mark it as not pressed
                if (Keyboard.GetState().IsKeyUp(key))
                    keyStates[key] = false;
            }
        }

        public static void HandlePlayerMovement(Player player)
        {
            int cellSize = DataStructure._constants.Settings.CellSize;

            // Define movement directions and their corresponding keys and move validation
            var movements = new (Keys key, int moveIndex, Vector2 direction)[]
            {
                (Keys.W, 0, new Vector2(0, -cellSize)), // Up
                (Keys.D, 1, new Vector2(cellSize, 0)), // Right
                (Keys.S, 2, new Vector2(0, cellSize)), // Down
                (Keys.A, 3, new Vector2(-cellSize, 0)), // Left
                (Keys.Up, 0, new Vector2(0, -cellSize)), // Up
                (Keys.Right, 1, new Vector2(cellSize, 0)), // Right
                (Keys.Down, 2, new Vector2(0, cellSize)), // Down
                (Keys.Left, 3, new Vector2(-cellSize, 0)) // Left
            };

            KeyboardState state = Keyboard.GetState();

            foreach (var (key, moveIndex, direction) in movements)
            {
                if (state.IsKeyDown(key) && Player.availableMoves[moveIndex])
                {
                    player.ApplyMovement(direction);
                    return;
                    // Prevents diagonal movement
                }
            }
        }
    }
}
