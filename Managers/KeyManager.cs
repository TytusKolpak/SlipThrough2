using System;
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
            string startView = ViewManager.viewsData.StartScreen.Name;
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
                            // Options off/on
                            if (ViewManager.currentView != optionsView)
                                ViewManager.SwitchView(optionsView);
                            else
                                ViewManager.SwitchView(mainView);
                            break;

                        case Keys.Q:
                            // Quit
                            if (ViewManager.currentView == optionsView)
                                game1.Exit();
                            break;

                        case Keys.Enter:
                            // Begin the game
                            if (ViewManager.currentView == startView)
                                ViewManager.SwitchView(mainView);
                            break;

                        case Keys.R:
                            // Reset (only if in Options)
                            if (ViewManager.currentView != optionsView)
                                return;

                            ViewManager.ResetViews(mainView);
                            break;

                        case Keys.S:
                            // Switch main map type
                            if (ViewManager.currentView == startView)
                                MapManager.ChangeMainMapType();
                            else if (ViewManager.currentView == optionsView)
                            {
                                MapManager.ChangeMainMapType();
                                ViewManager.ResetViews(mainView);
                            }

                            break;
                    }
                }

                // Check if the key is released and mark it as not pressed
                if (Keyboard.GetState().IsKeyUp(key))
                    keyStates[key] = false;
            }
        }

        public static void SetPlayerDirection(Player player)
        {
            int cellSize = DataStructure._constants.Settings.CellSize;
            // foreach (var move in Player.availableMoves)
            //     Console.Write($"{move}, ");

            // Console.WriteLine();

            // Movement directions with their corresponding keys and indices
            // Order is important because diagonal movement has to be checked first
            // otherwise it will be shadowed by any vertical/horizontal one
            var movements = new (Keys[] keys, int moveIndex, Vector2 direction)[]
            {
                // WDSA keys
                (new[] { Keys.W, Keys.D }, 0, new Vector2(cellSize, -cellSize)), // Right Up
                (new[] { Keys.D, Keys.S }, 1, new Vector2(cellSize, cellSize)), // Right Down
                (new[] { Keys.S, Keys.A }, 2, new Vector2(-cellSize, cellSize)), // Left Down
                (new[] { Keys.A, Keys.W }, 3, new Vector2(-cellSize, -cellSize)), // Left Up
                (new[] { Keys.W }, 4, new Vector2(0, -cellSize)), // Up
                (new[] { Keys.D }, 5, new Vector2(cellSize, 0)), // Right
                (new[] { Keys.S }, 6, new Vector2(0, cellSize)), // Down
                (new[] { Keys.A }, 7, new Vector2(-cellSize, 0)), // Left
                // Arrow Keys
                (new[] { Keys.Up }, 4, new Vector2(0, -cellSize)), // Up
                (new[] { Keys.Right }, 5, new Vector2(cellSize, 0)), // Right
                (new[] { Keys.Down }, 6, new Vector2(0, cellSize)), // Down
                (new[] { Keys.Left }, 7, new Vector2(-cellSize, 0)) // Left
            };

            KeyboardState state = Keyboard.GetState();

            foreach (var (keys, moveIndex, direction) in movements)
            {
                // Check if all required keys for the movement are pressed
                bool allKeysPressed = true;
                foreach (var key in keys)
                {
                    // If any of them is not pressed then dismiss (only makes sense for diagonal movement)
                    if (!state.IsKeyDown(key))
                    {
                        allKeysPressed = false;
                        break;
                    }
                }

                if (allKeysPressed && Player.availableMoves[moveIndex])
                {
                    player.direction = direction;
                    player.entityIsCooledDown = false;
                    return;
                }
            }
        }
    }
}
