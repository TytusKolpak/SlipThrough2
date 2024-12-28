using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;

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

            // Starting position is cell: (1,1) for example
            position = new Vector2(settingsData.CellSize * 10, settingsData.CellSize * 10);
            size = new(settingsData.CellSize, settingsData.CellSize);

            // -1 just means "for the player"
            AssignStats(-1, "Player");
        }

        public void Update()
        {
            int PCX = (int)position.X / settingsData.CellSize;
            int PCY = (int)position.Y / settingsData.CellSize;

            int[,] pattern = MapHandler.currentFunctionalPattern;

            bool IsValid(int y, int x)
            {
                // Player is within bounds (don't check outside the map)
                if (y < 0 || y >= settingsData.MapHeight || x < 0 || x >= settingsData.MapWidth)
                    return false;

                // Player wants to eneter a terrain cell (1) or door cell (-1, 1 and above)
                // Walls are 0
                return pattern[y, x] != 0;
            }

            // Define movement validity checks (order is important)
            availableMoves = new bool[]
            {
                IsValid(PCY - 1, PCX + 1), // Right Up
                IsValid(PCY + 1, PCX + 1), // Right Down
                IsValid(PCY + 1, PCX - 1), // Left Down
                IsValid(PCY - 1, PCX - 1), // Left Up
                IsValid(PCY - 1, PCX), // Up
                IsValid(PCY, PCX + 1), // Right
                IsValid(PCY + 1, PCX), // Down
                IsValid(PCY, PCX - 1), // Left
            };

            PerformMovement();

            // Might be here, might be in weapon manager
            WeaponManager.CheckForWeaponPickup(position);

            // Play sound if there is in fact some movement
            // (its always on but if the direction is 0,0 vector then there is no shift)
            bool keepPlayingSound = direction != Vector2.Zero;
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
            spriteBatch.Draw(
                texture: texture,
                destinationRectangle: new Rectangle(position.ToPoint(), size),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: 0.5f
            );
        }

        public void PerformAttack()
        {
            // If the player doesn't hold a or and isn't in an encounter - don't attack
            if (
                !WeaponManager.playerHoldsWeapon
                || MapHandler.mapName != Data.DataStructure._constants.Maps.EasyEncounter.Name
            )
                return;

            Console.Write("Performing attack! ");

            // Mark player as having just attacked, usted to temporary change the hitbox to attacking one
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
                    Vector2 enemyCellPostion =
                        new(
                            (float)Math.Round(enemy.position.X / settingsData.CellSize),
                            (float)Math.Round(enemy.position.Y / settingsData.CellSize)
                        );

                    Vector2 hitBoxCellPostion = WeaponManager.hitBoxPostion / cellSize;
                    if (hitBoxCellPostion == enemyCellPostion)
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
