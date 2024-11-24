using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using static SlipThrough2.Constants;

namespace SlipThrough2.Managers
{
    public class EnemyManager
    {
        private static List<Texture2D> textures;
        public static List<Enemy> Enemies;

        public EnemyManager(List<Texture2D> enemyTextures)
        {
            textures = enemyTextures;
        }

        public void Update()
        {
            if (Enemies == null)
                return;

            foreach (Enemy enemy in Enemies)
            {
                enemy.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enemies == null)
                return;

            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public static void SpawnEnemies(int doorNumber)
        {
            Enemies = new List<Enemy>();

            // Cover spawning only for these patterns which are declared
            if (ENEMY_SPAWN_PATTERN.Length < doorNumber)
                return;

            // First door has number 1, but array starts at 0
            doorNumber--;

            int[] enemiesInRoom = ENEMY_SPAWN_PATTERN[doorNumber];
            int numberOfEnemies = enemiesInRoom.Length;
            Texture2D[] enemyTextures = new Texture2D[numberOfEnemies];

            // Select only those for the current room
            for (int i = 0; i < numberOfEnemies; i++)
                enemyTextures[i] = textures[enemiesInRoom[i]];

            // Create them and add them to the list
            foreach (var texture in enemyTextures)
                Enemies.Add(new Enemy(texture, doorNumber));
        }
    }
}
