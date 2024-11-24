using System.Collections.Generic;
using SlipThrough2.Entities;
using SlipThrough2.Managers;
using static SlipThrough2.Constants;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        private static bool combatIsOver = false;
        private static int iteration,
            stage;

        public static void Update(Player player, List<Enemy> enemies)
        {
            // Next stage (of closing doors) every second
            if (combatIsOver)
            {
                iteration++;
                if (iteration == 30 && stage < 3)
                {
                    iteration = 0;
                    MapManager.OpenEncounterDoors(MapManager.allMaps[MAP_NAME.Encounter1], stage);
                    MapManager.SetMap(MAP_NAME.Encounter1);
                    stage++;
                }
            }

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

            if (!combatIsOver && enemies.Count == 0)
            {
                iteration = 0;
                stage = 0;
                combatIsOver = true;
            }
        }
    }
}
