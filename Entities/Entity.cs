using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Entities
{
    public abstract class Entity
    {
        public readonly int modifier = DataStructure._constants.Settings.TimeModifierConstant;
        public readonly int cellSize = DataStructure._constants.Settings.CellSize;
        public static readonly Settings settingsData = DataStructure._constants.Settings;
        public Texture2D texture;
        public Vector2 position,
            direction;
        public Point size;
        public string name;
        public int idleIterations,
            timeForShift,
            adjustedTimeForShift,
            attackCooldownIterations,
            recoveryIterations;
        public bool movementIsCooledDown,
            attackIsCoolingDown,
            wasJustHit,
            isDead;

        // Combat stats
        public int maxHealth,
            health,
            maxMana,
            mana,
            attack,
            speed;

        // For player it's -1 so that I do not need to make another method as overloaded method
        public void AssignStats(int doorNumber, string entityName)
        {
            EntityStructures entityData = DataStructure._constants.Entities;
            EntityStructure entity = doorNumber switch
            {
                -1 => entityData.Player,
                0 => entityData.EasyEnemy,
                1 => entityData.MediumEnemy,
                2 => entityData.HardEnemy,
                _ => throw new System.NotImplementedException()
            };

            maxHealth = entity.MaxHealth;
            health = entity.Health;
            maxMana = entity.MaxMana;
            mana = entity.Mana;
            attack = entity.Attack;
            speed = entity.Speed;
            name = entityName;

            /* We need to keep shift constant and operate on time for which the velocity is applied
            Shift = Velocity * Time => Time = Shift / Velocity*/
            timeForShift = modifier * cellSize / speed;
        }

        public void HandleMovementCooldown()
        {
            bool movementIsDiagonal = direction.X != 0 && direction.Y != 0;
            adjustedTimeForShift = movementIsDiagonal
                ? (int)(timeForShift * Math.Sqrt(2))
                : timeForShift;

            idleIterations++;

            if (idleIterations == adjustedTimeForShift)
            {
                /* This rounding is kinda funky but for now it's enough.
                It ensures that the entity is finishing its movement at the position
                which represents a cell in a grid, not somewhere in between cells */
                position = new Vector2(
                    (float)Math.Round(position.X / cellSize) * cellSize,
                    (float)Math.Round(position.Y / cellSize) * cellSize
                );
                direction = Vector2.Zero;
                idleIterations = 0;
                movementIsCooledDown = true;
            }
        }

        public void PerformMovement()
        {
            // Movement by nothing is still movement but no need to compute
            if (direction == Vector2.Zero)
                return;

            // And adjust shift for diagonal movement so that the speed stays roughly the same regardless of direction
            bool movementIsDiagonal = direction.X != 0 && direction.Y != 0;
            float diagonalFactor = movementIsDiagonal ? (float)(1 / Math.Sqrt(2)) : 1f;
            Vector2 shift = direction * speed / cellSize / modifier * diagonalFactor;

            // Performing the movement
            position += shift;
            movementIsCooledDown = false;

            // Only for ther player if he is moving (idle iterations do stay at 0 when not moving)
            if (this is Player && idleIterations != 0)
            {
                int heightInStepModifier = idleIterations < timeForShift / 2 ? 1 : -1; // 1,1,1,1,-1,-1,-1,-1
                position.Y -= heightInStepModifier; // - because Y axis is upside down
            }
        }

        public void HandleAttackCooldown()
        {
            attackCooldownIterations--;

            if (attackCooldownIterations == 0)
                attackIsCoolingDown = false;
        }
    }
}
