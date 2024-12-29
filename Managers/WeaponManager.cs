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
        private static readonly float scale = 2;
        public static Vector2 hitBoxPosition,
            origin = new(8, 8),
            adjuster = origin * scale;
        private Vector2 position;

        public static bool playerHoldsWeapon,
            enemyInHitBox;
        public static float layerDepth = 0.1f,
            rotation;
        public static string directionX;

        public WeaponManager(List<Texture2D> weaponTextures)
        {
            TEXTURES = weaponTextures;
            SetWeaponParameters("Sword");
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
            Vector2 playerPosition,
            bool playerJustAttacked,
            Vector2 attackDirection
        )
        {
            DisplayWeapon(batch, playerPosition);
            DisplayWeaponHitBox(batch, playerJustAttacked, attackDirection);
        }

        public void CheckForWeaponPickup(Vector2 playerPosition)
        {
            if (
                !playerHoldsWeapon
                && position.X == playerPosition.X
                && position.Y == playerPosition.Y
            )
            {
                Console.WriteLine($"Player picks up a weapon at: {position}");

                // Attach its position to the player (+ offsets and rotation)
                playerHoldsWeapon = true;
                rotation = MathHelper.ToRadians(45);
                directionX = "right";
            }
        }

        private void SetWeaponParameters(string name)
        {
            weaponIndex = weaponData.FindIndex(element => element.Name == name);
            position = Player.ChooseStartingPosition();
        }

        private void DisplayWeapon(SpriteBatch batch, Vector2 playerPosition)
        {
            // Don't draw if player is in encounter while not holding a weapon
            if (MapHandler.mapName == mapsData.EasyEncounter.Name && !playerHoldsWeapon)
                return;

            // Overwrite the original weapon location - make player carry the weapon
            if (playerHoldsWeapon)
            {
                position = playerPosition;
                position.Y += 5;

                if (directionX == "right")
                {
                    position.X += 20;
                    rotation = MathHelper.ToRadians(45);
                }

                if (directionX == "left")
                {
                    position.X -= 20;
                    rotation = MathHelper.ToRadians(-45);
                }

                layerDepth = 0.6f; // Just above the player
            }

            batch.Draw(
                texture: TEXTURES[weaponIndex],
                position: position + adjuster,
                sourceRectangle: null,
                color: Color.White,
                rotation: rotation,
                origin: origin,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: layerDepth
            );
        }

        private void DetermineHitBoxPosition(Vector2 playerPosition, Vector2 attackDirection)
        {
            if (!playerHoldsWeapon || MapHandler.mapName != mapsData.EasyEncounter.Name)
                return;

            // Change from smooth to by-cell position
            playerPosition.X = (float)Math.Round(playerPosition.X / cellSize);
            playerPosition.Y = (float)Math.Round(playerPosition.Y / cellSize);

            // Back to coordinates position after adding direction of attack
            hitBoxPosition = (playerPosition + attackDirection) * cellSize;
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
            Vector2 enemyCellPosition;
            foreach (Enemy enemy in enemies)
            {
                enemyCellPosition = Vector2.Divide(enemy.position, cellSize);
                enemyCellPosition.Round(); // Is this necessary?

                // If the hitbox is over enemy mark it as so (later it changes the hitbox icon)
                Vector2 hitBoxCellPosition = Vector2.Divide(hitBoxPosition, cellSize);
                hitBoxCellPosition.Round();

                enemyInHitBox = hitBoxCellPosition == enemyCellPosition;

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

            // Draw selected hitbox
            batch.Draw(
                texture: TEXTURES[hitBoxIndex],
                position: hitBoxPosition,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0.3f // Above enemy
            );
        }
    }
}
