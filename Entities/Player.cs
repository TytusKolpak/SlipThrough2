using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;
using SlipThrough2.Views;

namespace SlipThrough2.Entities
{
    // Player is a player and a playerManager at the same time
    public class Player : Entity
    {
        public static bool[] availableMoves;
        public Vector2 attackDirection;

        public Player(Texture2D playerTexture)
        {
            texture = playerTexture;
            position = ChooseStartingPosition();
            size = new(settingsData.CellSize, settingsData.CellSize);

            // -1 just means "for the player"
            AssignStats(-1, "Player");
        }

        public void Update()
        {
            // Disable movement and change color for a while after being hit
            if (wasJustHit)
            {
                HandleRecovery();
                return; // We want to stop the movement but also the movement cooldown
            }

            CheckAvailableMoves();
            PerformMovement();

            // Play sound if there is in fact some movement
            // (its always on but if the direction is 0,0 vector then there is no shift)
            bool keepPlayingSound = false;
            if (direction != Vector2.Zero && !MainGame.isGameOver)
                keepPlayingSound = true;

            AudioManager.PlayLoopedWalkingSound(keepPlayingSound);

            // If the player has just attacked disable them from attacking
            if (attackIsCoolingDown)
                HandleAttackCooldown();
            else
                KeyManager.HandlePlayerAttackKeys(this);

            if (!movementIsCooledDown)
            {
                HandleMovementCooldown();
                return;
            }

            KeyManager.HandlePlayerMovementKeys(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (wasJustHit)
                color = Color.Red;
            else
                color = Color.White;
            
            DrawEntity(spriteBatch, layerDepth: 0.5f);
        }

        public static Vector2 ChooseStartingPosition()
        {
            Random rnd = new();
            Vector2 tmpPosition = Vector2.Zero;

            // Select one of the cells at random, if it's on the ground, accept oi
            while (tmpPosition == Vector2.Zero)
            {
                int x = (int)(rnd.NextInt64() % (settingsData.MapWidth - 1));
                int y = (int)(rnd.NextInt64() % (settingsData.MapHeight - 1));

                if (MapHandler.currentFunctionalPattern[y, x] == 1)
                    tmpPosition = new(x * settingsData.CellSize, y * settingsData.CellSize);
            }
            return tmpPosition;
        }

        private static bool IsMoveValid(int y, int x)
        {
            // Player is within bounds (don't check outside the map)
            if (y < 0 || y >= settingsData.MapHeight || x < 0 || x >= settingsData.MapWidth)
                return false;

            // Player wants to enter a terrain cell (1) or door cell (-1, 1 and above), walls are 0
            return MapHandler.currentFunctionalPattern[y, x] != 0;
        }

        private void CheckAvailableMoves()
        {
            int PCX = (int)position.X / cellSize;
            int PCY = (int)position.Y / cellSize;

            // Define movement validity checks (order is important)
            availableMoves = new bool[]
            {
                IsMoveValid(PCY - 1, PCX + 1), // Right Up
                IsMoveValid(PCY + 1, PCX + 1), // Right Down
                IsMoveValid(PCY + 1, PCX - 1), // Left Down
                IsMoveValid(PCY - 1, PCX - 1), // Left Up
                IsMoveValid(PCY - 1, PCX), // Up
                IsMoveValid(PCY, PCX + 1), // Right
                IsMoveValid(PCY + 1, PCX), // Down
                IsMoveValid(PCY, PCX - 1), // Left
            };
        }

        public void PerformAttack()
        {
            // If the player doesn't hold a or and isn't in an encounter - don't attack
            if (
                !WeaponManager.playerHoldsWeapon
                || MapHandler.mapName != DataStructure._constants.Maps.EasyEncounter.Name
            )
                return;

            Console.Write("Performing attack! ");
            AudioManager.PlaySoundOnce("attack");

            // Mark player as having just attacked, used to temporary change the hitbox to attacking one
            attackIsCoolingDown = true;
            attackCooldownIterations = 20; // about 0.33s

            if (WeaponManager.enemyInHitBox)
            {
                Console.WriteLine("Hit!");

                // Iterate through all enemies
                int numberOfEnemies = EnemyManager.Enemies.Count;
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    Enemy enemy = EnemyManager.Enemies[i];

                    // Check if this is the one who is being hit
                    Vector2 enemyCellPosition =
                        new(
                            (float)Math.Round(enemy.position.X / settingsData.CellSize),
                            (float)Math.Round(enemy.position.Y / settingsData.CellSize)
                        );

                    Vector2 hitBoxCellPosition = WeaponManager.hitBoxPosition / cellSize;
                    if (hitBoxCellPosition == enemyCellPosition)
                    {
                        enemy.ReceiveDamage(attack);

                        Console.WriteLine(
                            $"Player deals {attack} damage to {enemy.name}, leaving it with {enemy.health}/{enemy.maxHealth} health."
                        );
                    }
                }
            }
            else
            {
                Console.WriteLine("Miss.");
            }
        }
    }
}
