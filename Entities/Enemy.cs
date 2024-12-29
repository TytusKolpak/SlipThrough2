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
            if (isDead)
                return;

            // Disable movement and change color for a while after being hit
            if (wasJustHit)
            {
                HandleRecovery();
                return; // We want to stop the movement but also the movement cooldown
            }

            PerformMovement();

            // Right now the enemy attacks player by standing on them
            if (attackIsCoolingDown)
            {
                // This is not used here due to what the enemy does after attack (moves away)
                // but when their behavior will change this might become necessary
                HandleAttackCooldown();
            }
            else
            {
                if (player.position == position)
                    PerformAttack(player);
            }

            if (!movementIsCooledDown)
                HandleMovementCooldown();
            else
                DetermineDirection(player.position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (wasJustHit)
                color = Color.Red;
            else
                color = Color.White;

            if (isDead)
            {
                color = Color.Gray;
                rotation = MathHelper.ToRadians(90);
                layerDepth = 0.19f; // Under the not yet dead enemies this is a typo2sa
            }
            else
                layerDepth = 0.2f;

            DrawEntity(spriteBatch, layerDepth);

            // No need to draw health if we know it's empty
            if (!isDead)
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
            // Affect the player
            player.health -= attack;
            player.wasJustHit = true;
            player.recoveryIterations = 6;

            // Mark enemy as having just attacked
            attackIsCoolingDown = true;
            attackCooldownIterations = 20; // about 0.33s
        }

        public void ReceiveDamage(int damage)
        {
            // Remove x life from enemy
            health -= damage;

            // Used to make the texture get red hue
            wasJustHit = true;

            // Time for the color change
            recoveryIterations = 6;
        }
    }
}
