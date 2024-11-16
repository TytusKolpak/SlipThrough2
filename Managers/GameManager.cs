using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        public MapManager MapManager;
        public EnemyManager EnemyManager;
        public Player Player;

        // Move to Enemies.cs and handle there along with Spawning at certain time

        public GameManager(
            Texture2D playerTexture,
            List<Texture2D> enemyTextures,
            List<Texture2D> mapTextures,
            List<Texture2D> HUDTextures,
            SpriteFont font
        )
        {
            MapManager = new MapManager(mapTextures);
            EnemyManager = new EnemyManager(enemyTextures);
            Player = new Player(playerTexture, HUDTextures, font);
        }

        public void Update()
        {
            Player.Update(MapManager.MapHandler.roomName);
            EnemyManager.Update();
            MapManager.Update(Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            MapManager.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);
            Player.Draw(spriteBatch, MapManager.MapHandler.roomName);

            spriteBatch.End();
        }
    }
}
