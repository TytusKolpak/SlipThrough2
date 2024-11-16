using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        public Player Player;
        public static List<Enemy> Enemies;
        public static List<Texture2D> enemyTextureList;
        public MapManager MapManager;

        public GameManager(
            Texture2D playerTexture,
            List<Texture2D> enemyTextures,
            List<Texture2D> mapTextures,
            List<Texture2D> HUDTextures,
            SpriteFont font
        )
        {
            MapManager = new MapManager(mapTextures);
            Player = new Player(playerTexture, HUDTextures, font);
            enemyTextureList = enemyTextures;
        }

        public void Update()
        {
            Player.Update(MapManager.MapHandler.roomName);

            if (Enemies != null)
            {
                foreach (var enemy in Enemies)
                {
                    enemy.Update();
                }
            }

            MapManager.Update(Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            MapManager.Draw(spriteBatch);

            if (Enemies != null)
            {
                foreach (var enemy in Enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }

            Player.Draw(spriteBatch, MapManager.MapHandler.roomName);

            spriteBatch.End();
        }

        public static void SpawnEnemies()
        {
            Enemies = new List<Enemy>();
            foreach (var texture in enemyTextureList)
            {
                Enemies.Add(new Enemy(texture));
            }
        }
    }
}
