using System.Collections.Generic;
using SlipThrough2.Entities;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        public static void Update(Player player, List<Enemy> enemies)
        {
            if (!player.entityIsCooledDown)
                return;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (player.position == enemy.position)
                    enemy.health -= player.attack;

                if (enemy.health == 0)
                    enemies.RemoveAt(i);
            }
        }
    }
}
