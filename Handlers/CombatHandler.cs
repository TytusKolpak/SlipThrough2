using System.Collections.Generic;
using SlipThrough2.Entities;
using SlipThrough2.Managers;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        public static bool combatIsOver;

        public static void Update(List<Enemy> enemies)
        {
            // Next stage (of closing doors) every second
            if (combatIsOver)
                MapManager.HandleOpeningDoors();

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                // Enemy dies
                if (enemy.health == 0)
                {
                    enemies.RemoveAt(i);
                    AudioManager.PlaySoundOnce("death");
                }
            }

            combatIsOver = !combatIsOver && enemies.Count == 0;
        }

        public static void ResetCombatParameters()
        {
            combatIsOver = false;
        }
    }
}
