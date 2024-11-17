using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using SlipThrough2.Managers;
using static SlipThrough2.Constants;

namespace SlipThrough2.Views
{
    public class MainGame : View
    {
        private readonly MapManager MapManager;
        private readonly EnemyManager EnemyManager;
        private readonly Player Player;

        public MainGame(
            VIEW_NAME viewName,
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                SpriteFont Font,
                SpriteBatch spriteBatch
            ) gameAssets
        )
        {
            view = viewName;

            MapManager = new MapManager(gameAssets.MapTextures);
            EnemyManager = new EnemyManager(gameAssets.EnemyTextures);
            Player = new Player(gameAssets.PlayerTexture, gameAssets.HUDTextures, gameAssets.Font);
        }

        public override void Update()
        {
            MapManager.Update(Player);
            EnemyManager.Update();
            Player.Update(MapManager.MapHandler.roomName);
        }

        public override void Draw()
        {
            MapManager.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);
            Player.Draw(spriteBatch, MapManager.MapHandler.roomName);
        }
    }
}
