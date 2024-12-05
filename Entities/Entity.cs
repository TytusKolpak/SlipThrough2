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

        // For texture handling
        public Texture2D texture;
        public Vector2 position,
            direction;

        // For movement logic
        public int idleIterations,
            timeForShift;
        public bool entityIsCooledDown;

        // Combat stats
        public int maxHealth,
            health,
            maxMana,
            mana,
            attack,
            speed;

        // For player it's -1 so that I do not need to make another method as overloaded method
        public void AssignStats(int doorNumber)
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

            /* We need to keep shift constant and operate on time for which the velocity is applied
            Shift = Velocity * Time => Time = Shift / Velocity*/
            timeForShift = modifier * cellSize / speed;
        }

        public void HandleCooldown()
        {
            idleIterations++;
            if (idleIterations == timeForShift)
            {
                /* This rounding is kinda funky but for now it's enough.
                It ensures that the entity is finishing its movement at the position
                which represents a cell in a grid, not somewhere in between cells */
                position = new Vector2(
                    (float)Math.Round(position.X / cellSize) * cellSize,
                    (float)Math.Round(position.Y / cellSize) * cellSize
                );
                direction = new Vector2(0, 0);
                idleIterations = 0;
                entityIsCooledDown = true;
            }
        }
    }
}
