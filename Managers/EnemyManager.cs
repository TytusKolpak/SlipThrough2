using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class EnemyManager
    {
        private static List<Texture2D> textures;
        private static List<Enemy> Enemies;

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

        public static void SpawnEnemies()
        {
            Enemies = new List<Enemy>();
            foreach (var texture in textures)
            {
                Enemies.Add(new Enemy(texture));
            }
        }
    }
}
