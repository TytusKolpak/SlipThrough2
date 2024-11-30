using System.Collections.Generic;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Managers;

namespace SlipThrough2.Handlers
{
    public class CombatHandler
    {
        private static bool combatIsOver;
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
                    stage++;
                    MapManager.OpenEncounterDoors(stage);
                    MapManager.SetMap(DataStructure._constants.Maps.EasyEncounter.Name);
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

        public static void ResetCombatParameters()
        {
            combatIsOver = false;
            iteration = 0;
            stage = 0;
        }
    }
}
