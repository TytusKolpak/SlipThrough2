using System.Collections.Generic;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Managers;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        public static bool combatIsOver;

        public static void Update(Player player, List<Enemy> enemies)
        {
            // Next stage (of closing doors) every second
            if (combatIsOver)
                MapManager.HandleOpeningDoors();

            // This is funky, change approach
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

            combatIsOver = !combatIsOver && enemies.Count == 0;
        }

        public static void ResetCombatParameters()
        {
            combatIsOver = false;
        }
    }
}
