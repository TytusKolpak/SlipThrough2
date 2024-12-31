using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;

namespace SlipThrough2.Managers
{
    public class KeyManager
    {
        private static readonly Dictionary<Keys, bool> isPressed = new();
        private static bool attackKeyPressedDown;

        public static void Update(Game1 game1)
        {
            HandleGoverningKeys(game1);
        }

        private static void HandleGoverningKeys(Game1 game1)
        {
            List<TrackedKey> trackedKeys = DataStructure._constants.Settings.TrackedKeys;
            string optionsView = ViewManager.viewsData.Options.Name;
            string startView = ViewManager.viewsData.StartScreen.Name;
            string mainView = ViewManager.viewsData.MainGame.Name;

            foreach (var trackedKey in trackedKeys)
            {
                Keys key = (Keys)trackedKey.Value;

                // Check if the key is pressed and was not previously pressed
                if (Keyboard.GetState().IsKeyDown(key) && !isPressed[key])
                {
                    isPressed[key] = true; // Mark the value as being pressed

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

                        case Keys.E:
                            AudioManager.enableSoundEffects = !AudioManager.enableSoundEffects;
                            if (AudioManager.enableSoundEffects)
                                AudioManager.PlaySoundOnce("door");
                            break;
                    }
                }

                // Check if the key is released and mark it as not pressed
                if (Keyboard.GetState().IsKeyUp(key))
                    isPressed[key] = false;
            }
        }

        public static void HandlePlayerMovementKeys(Player player)
        {
            int cellSize = DataStructure._constants.Settings.CellSize;

            // Movement directions with their corresponding keys and indices
            // Order is important because diagonal movement has to be checked first
            // otherwise it will be shadowed by any vertical/horizontal one
            var movements = new (Keys[] keys, int moveIndex, Vector2 direction)[]
            {
                // W,D,S,A keys
                (new[] { Keys.W, Keys.D }, 0, new Vector2(cellSize, -cellSize)), // Right Up
                (new[] { Keys.D, Keys.S }, 1, new Vector2(cellSize, cellSize)), // Right Down
                (new[] { Keys.S, Keys.A }, 2, new Vector2(-cellSize, cellSize)), // Left Down
                (new[] { Keys.A, Keys.W }, 3, new Vector2(-cellSize, -cellSize)), // Left Up
                (new[] { Keys.W }, 4, new Vector2(0, -cellSize)), // Up
                (new[] { Keys.D }, 5, new Vector2(cellSize, 0)), // Right
                (new[] { Keys.S }, 6, new Vector2(0, cellSize)), // Down
                (new[] { Keys.A }, 7, new Vector2(-cellSize, 0)), // Left
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
                    return;
                }
            }
        }

        public static void HandlePlayerAttackKeys(Player player)
        {
            if (
                MapHandler.mapName != DataStructure._constants.Maps.EasyEncounter.Name
                || !WeaponManager.playerHoldsWeapon
            )
                return;

            KeyboardState state = Keyboard.GetState();

            // Set attacking direction
            if (state.IsKeyDown(Keys.J))
                player.attackDirection.X = -1;
            else if (state.IsKeyDown(Keys.L))
                player.attackDirection.X = 1;
            else
                player.attackDirection.X = 0;

            if (state.IsKeyDown(Keys.I))
                player.attackDirection.Y = -1;
            else if (state.IsKeyDown(Keys.K))
                player.attackDirection.Y = 1;
            else
                player.attackDirection.Y = 0;

            // Check if attacking
            if (state.IsKeyDown(Keys.Space) && !attackKeyPressedDown)
            {
                attackKeyPressedDown = true;
                player.PerformAttack();
            }

            if (state.IsKeyUp(Keys.Space))
                attackKeyPressedDown = false;
        }
    }
}
