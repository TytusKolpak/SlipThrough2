using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Managers;

namespace SlipThrough2.Entities
{
    public class Enemy : Entity
    {
        public Enemy(Texture2D enemyTexture, int doorNumber, string entityName)
        {
            texture = enemyTexture;
            position = new Vector2(settingsData.WindowWidth / 2, settingsData.WindowHeight / 2); // Starting position, for example
            size = new(settingsData.CellSize, settingsData.CellSize);
            AssignStats(doorNumber, entityName);
        }

        public void Update(Player player)
        {
            PerformMovement();

            // Right now the enemy attacks player by standing on them
            if (attackIsCoolingDown)
                HandleAttackCooldown();
            else
            {
                if (player.position == position)
                    PerformAttack(player);
            }

            if (!movementIsCooledDown)
            {
                HandleMovementCooldown();
                return;
            }

            DetermineDirection(player.position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the enemy itself
            spriteBatch.Draw(
                texture: texture,
                destinationRectangle: new Rectangle(position.ToPoint(), size),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: 0.2f // Under everything but the ground which is layer 0 (and weapns on ground)
            );

            HUDManager.DisplayEnemyHealthBar(spriteBatch, position, health, maxHealth);
        }

        private void DetermineDirection(Vector2 playerPosition)
        {
            // Distance between this enemy and the player (in cells)
            float distance = Vector2.Distance(position, playerPosition) / settingsData.CellSize;

            if (distance <= 3 && distance != 0)
            {
                // If the enemy is near the player - move towards them
                Vector2 directionToPlayer = Vector2.Subtract(playerPosition, position);
                directionToPlayer.Normalize();
                directionToPlayer.Round(); // We need direction in 1/0 pattern
                direction = directionToPlayer * settingsData.CellSize;
            }
            else
            {
                // Generate random direction
                Random rnd = new();
                direction = new(
                    settingsData.CellSize * (rnd.Next(3) - 1),
                    settingsData.CellSize * (rnd.Next(3) - 1)
                );
            }
        }

        private void PerformAttack(Player player)
        {
            player.health -= attack;

            // Mark enemy as having just attacked
            attackIsCoolingDown = true;
            attackCooldownIterations = 20; // about 0.33s
        }
    }
}
