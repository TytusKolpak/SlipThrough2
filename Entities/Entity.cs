using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public abstract class Entity
    {
        // For texture handling
        public Texture2D texture;
        public Vector2 position;

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
            string entity = doorNumber switch
            {
                -1 => "Player",
                0 => "EasyEnemy",
                1 => "MediumEnemy",
                2 => "HardEnemy",
                _ => throw new System.NotImplementedException()
            };

            maxHealth = STATS[entity]["maxHealth"];
            health = STATS[entity]["health"];
            maxMana = STATS[entity]["maxMana"];
            mana = STATS[entity]["mana"];
            attack = STATS[entity]["attack"];
        }

        public void HandleCooldown()
        {
            int multiplier = this is Enemy ? 4 : 1;

            idleIterations++;
            if (idleIterations > ITERATION_TIME * multiplier)
            {
                idleIterations = 0;
                entityIsCooledDown = true;
            }
        }
    }
}
