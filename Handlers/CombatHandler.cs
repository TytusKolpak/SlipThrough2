using System.Collections.Generic;
using System.Linq;
using SlipThrough2.Entities;
using SlipThrough2.Managers;
using SlipThrough2.Views;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        public static bool combatIsOver;

        public void Update(List<Enemy> enemies, Player player)
        {
            // Next stage (of closing doors) every second
            if (combatIsOver)
                MapManager.HandleOpeningDoors();
            else
            {
                // combat is over when all enemies are dead
                combatIsOver = enemies.All(enemy => enemy.isDead);
            }

            // Check player death
            if (player.health <= 0)
            {
                player.isDead = true;
                MainGame.isGameOver = true;
            }

            CheckEnemyDeaths(enemies);
        }

        public static void ResetCombatParameters()
        {
            combatIsOver = false;
        }

        private void CheckEnemyDeaths(List<Enemy> enemies)
        {
            // Check for death of all the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                // Don't check if this enemy is dead if we know that they are already dead
                if (enemy.isDead)
                    continue;

                // wasJustHit is here to wait a bit before removing enemy
                if (enemy.health <= 0 && !enemy.wasJustHit)
                {
                    // Enemy dies
                    System.Console.WriteLine($"Enemy: {enemy.name} is Dead!");
                    enemy.isDead = true;
                    AudioManager.PlaySoundOnce("death");
                }
            }
        }
    }
}
