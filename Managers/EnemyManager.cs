using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;

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

        public void Update(Player player)
        {
            if (Enemies == null)
                return;

            foreach (Enemy enemy in Enemies)
                enemy.Update(player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enemies == null)
                return;

            foreach (Enemy enemy in Enemies)
                enemy.Draw(spriteBatch);
        }

        public static void SpawnEnemies(int doorNumber)
        {
            Enemies = new List<Enemy>();

            string[][] EnemySet = DataStructure._constants.Encounters.EnemySet;
            List<Tile> AllEnemies = DataStructure._constants.Tiles.Enemy;

            // Cover spawning only for these patterns which are declared
            if (EnemySet.Length < doorNumber)
                return;

            // First door has number 1, but array starts at 0
            doorNumber--;

            string[] enemiesInRoom = EnemySet[doorNumber];
            int numberOfEnemies = enemiesInRoom.Length;
            Texture2D[] enemyTextures = new Texture2D[numberOfEnemies];

            // Select only those for the current room
            for (int i = 0; i < numberOfEnemies; i++)
            {
                string currentEnemy = enemiesInRoom[i];
                int textureIndex = AllEnemies.FindIndex(enemy => enemy.Name == currentEnemy);

                // Create them and add them to the list
                Enemies.Add(
                    new Enemy(
                        enemyTexture: textures[textureIndex],
                        doorNumber,
                        entityName: currentEnemy
                    )
                );
            }
        }
    }
}
