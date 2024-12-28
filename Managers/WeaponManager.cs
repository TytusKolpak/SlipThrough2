using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;

namespace SlipThrough2.Managers
{
    class WeaponManager
    {
        private readonly List<Tile> weaponData = DataStructure._constants.Tiles.Weapon;
        private readonly int cellSize = DataStructure._constants.Settings.CellSize;
        private static readonly Maps mapsData = DataStructure._constants.Maps;
        private readonly List<Texture2D> TEXTURES;
        private int weaponIndex,
            hitBoxIndex;
        public static Rectangle rectangle;
        private Point size,
            adjustedPlayerPosition;

        public static bool playerHoldsWeapon,
            enemyInHitBox;
        public static float layerDepth = 0.1f,
            rotation;
        public static string directionX;
        public static Vector2 hitBoxPostion;

        public WeaponManager(List<Texture2D> weaponTextures)
        {
            TEXTURES = weaponTextures;
            SetWeaponParameters(11, 10, "Sword");
        }

        public void Update(
            Vector2 playerPosition,
            Vector2 playerDirection,
            Vector2 attackDirection,
            List<Enemy> enemies
        )
        {
            DetermineHitBoxPosition(playerPosition, attackDirection);
            
            if (!playerHoldsWeapon)
                CheckForWeaponPickup(playerPosition);

            CheckEnemiesInHitBox(enemies);
            DetermineWeaponPosition(playerDirection);
        }

        public void Draw(
            SpriteBatch batch,
            Vector2 playerPositon,
            bool playerJustAttacked,
            Vector2 attackDirection
        )
        {
            DisplayWeapon(batch, playerPositon);
            DisplayWeaponHitBox(batch, playerJustAttacked, attackDirection);
        }

        public static void CheckForWeaponPickup(Vector2 position)
        {
            if (!playerHoldsWeapon && rectangle.X == position.X && rectangle.Y == position.Y)
            {
                Console.WriteLine($"Player picks up a weapon at: {rectangle}");

                // Attach its position to the player (+ offsets and rotation)
                playerHoldsWeapon = true;
                rotation = MathHelper.ToRadians(45);
                directionX = "right";
            }
        }

        private void SetWeaponParameters(int x, int y, string name)
        {
            weaponIndex = weaponData.FindIndex(element => element.Name == name);
            Point position = Player.ChooseStartingPosition().ToPoint();
            size = new(cellSize, cellSize);
            rectangle = new(position, size);
        }

        private void DisplayWeapon(SpriteBatch batch, Vector2 pPositon)
        {
            if (
                MapHandler.mapName == mapsData.NewMain.Name
                || (playerHoldsWeapon && MapHandler.mapName == mapsData.EasyEncounter.Name)
            )
            {
                // Overwrite the original weapon location - make player carry the weapon
                if (playerHoldsWeapon)
                {
                    if (directionX == "right")
                    {
                        adjustedPlayerPosition = new((int)pPositon.X + cellSize, (int)pPositon.Y);
                        rotation = MathHelper.ToRadians(45);
                    }

                    if (directionX == "left")
                    {
                        adjustedPlayerPosition = new(
                            (int)pPositon.X - cellSize * 3 / 4,
                            (int)pPositon.Y + cellSize * 3 / 4
                        );
                        rotation = MathHelper.ToRadians(-45);
                    }

                    rectangle = new Rectangle(adjustedPlayerPosition, size);
                    layerDepth = 0.6f; // Just above the player
                }

                batch.Draw(
                    texture: TEXTURES[weaponIndex],
                    destinationRectangle: rectangle,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: rotation,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: layerDepth // Above ground, below enemy
                );
            }
        }

        private void DetermineHitBoxPosition(Vector2 playerPosition, Vector2 attackDirection)
        {
            if (!playerHoldsWeapon || MapHandler.mapName != mapsData.EasyEncounter.Name)
                return;

            // Change from smooth to by-cell position
            playerPosition.X = (float)Math.Round(playerPosition.X / cellSize);
            playerPosition.Y = (float)Math.Round(playerPosition.Y / cellSize);

            // Back to coordinates position after adding direction of attack
            hitBoxPostion = (playerPosition + attackDirection) * cellSize;
        }

        private void DetermineWeaponPosition(Vector2 playerDirection)
        {
            if (playerDirection.X == cellSize)
                directionX = "right";
            else if (playerDirection.X == -cellSize)
                directionX = "left";
        }

        private void CheckEnemiesInHitBox(List<Enemy> enemies)
        {
            if (enemies == null)
                return;

            enemyInHitBox = false;
            Vector2 enemyCellPostion;
            foreach (Enemy enemy in enemies)
            {
                enemyCellPostion = Vector2.Divide(enemy.position, cellSize);
                enemyCellPostion.Round(); // Is this necessary?

                // If the hitbox is over enemy mark it as so (later it changes the hitbox icon)
                Vector2 hitBoxCellPostion = Vector2.Divide(hitBoxPostion, cellSize);
                hitBoxCellPostion.Round();

                enemyInHitBox = hitBoxCellPostion == enemyCellPostion;

                // No need to check further if one is already inside
                if (enemyInHitBox)
                    break;
            }
        }

        private void DisplayWeaponHitBox(
            SpriteBatch batch,
            bool pJustAttacked,
            Vector2 attackDirection
        )
        {
            // If the player doesn't hold a or and isn't in an encounter - no hitbox
            if (
                !playerHoldsWeapon
                || MapHandler.mapName != mapsData.EasyEncounter.Name
                || attackDirection == Vector2.Zero
            )
                return;

            string hitBoxType;

            // Display a Hit regardless of the hitBox "status" if the player attacks
            if (pJustAttacked)
                hitBoxType = "Hit";
            else
            {
                // Set the hit box index to that of hitting if there is an enemy in it, otherwise missing
                if (enemyInHitBox)
                    hitBoxType = "Hitting HitBox";
                else
                    hitBoxType = "Missing HitBox";
            }
            hitBoxIndex = weaponData.FindIndex(element => element.Name == hitBoxType);

            Rectangle hitBoxRectangle = new(hitBoxPostion.ToPoint(), size);

            // Draw selected hitbox
            batch.Draw(
                texture: TEXTURES[hitBoxIndex],
                destinationRectangle: hitBoxRectangle,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: 0.3f // Above enemy
            );
        }
    }
}
