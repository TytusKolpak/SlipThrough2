using System.Collections.Generic;
using System.Linq;
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
            else
            {
                // combat is over when all enemies are dead
                combatIsOver = enemies.All(enemy => enemy.isDead);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                // Dont check if this enemy is dead if we know that they are already dead
                if (enemy.isDead)
                    continue;

                // wasJustHit is here to wait a bit before removing enemy
                if (enemy.health == 0 && !enemy.wasJustHit)
                {
                    // Enemy dies
                    System.Console.WriteLine($"Enemy: {enemy.name} is Dead!");
                    enemy.isDead = true;
                    AudioManager.PlaySoundOnce("death");
                }
            }
        }

        public static void ResetCombatParameters()
        {
            combatIsOver = false;
        }
    }
}
