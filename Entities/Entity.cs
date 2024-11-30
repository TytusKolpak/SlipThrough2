using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Entities
{
    public abstract class Entity
    {
        // For texture handling
        public Texture2D texture;
        public Vector2 position; // field

        // For movement logic
        private int idleIterations;
        public bool entityIsCooledDown;

        // Combat stats
        public int maxHealth,
            health,
            maxMana,
            mana,
            attack;

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
        }

        public void HandleCooldown()
        {
            int multiplier = this is Enemy ? 8 : 1;

            idleIterations++;
            if (idleIterations > DataStructure._constants.Settings.IterationTime * multiplier)
            {
                idleIterations = 0;
                entityIsCooledDown = true;
            }
        }
    }
}
