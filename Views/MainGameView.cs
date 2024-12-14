using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;

namespace SlipThrough2.Views
{
    public class MainGame : View
    {
        private readonly MapManager MapManager;
        private EnemyManager EnemyManager;
        private readonly Player Player;
        private HUDManager HUDManager;
        public bool playerInEncounter;

        public MainGame(
            string viewName,
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
            Player = new Player(gameAssets.PlayerTexture);
            HUDManager = new HUDManager(gameAssets.HUDTextures, gameAssets.Font, Player);
        }

        public override void Update()
        {
            MapManager.Update(Player);
            EnemyManager.Update();
            Player.Update();

            playerInEncounter = MapHandler.mapName == Data.DataStructure._constants.Maps.EasyEncounter.Name;

            if (playerInEncounter)
            {
                HUDManager.Update();
                CombatHandler.Update(Player, EnemyManager.Enemies);
            }
        }

        public override void Draw()
        {
            MapManager.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);

            if (playerInEncounter)
                HUDManager.Draw(spriteBatch);
        }

        // Here we assume that Remove affects The enemies and their manager
        public override void Remove()
        {
            EnemyManager = null;
            EnemyManager.Enemies = null;
            HUDManager = null;
            HUDManager.healthBarTilePattern = null;
            HUDManager.manaBarTilePattern = null;
            HUDManager.healthBarTextures = new();
            HUDManager.manaBarTextures = new();
            HUDManager.iteration = 0;
            CombatHandler.ResetCombatParameters();
        }
    }
}
