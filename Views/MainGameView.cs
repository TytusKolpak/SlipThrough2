using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;
using SlipThrough2.Handlers;
using SlipThrough2.Managers;

namespace SlipThrough2.Views
{
    public class MainGame : View
    {
        private readonly MapManager MapManager;
        private EnemyManager EnemyManager;
        private Player Player;
        private HUDManager HUDManager;
        private WeaponManager WeaponManager;
        private CombatHandler CombatHandler;

        private Settings settingsData = DataStructure._constants.Settings;
        private SpriteFont spriteFont;
        public bool playerInEncounter;
        public static bool isGameOver;

        public MainGame(
            string viewName,
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                List<Texture2D> WeaponTextures,
                SpriteFont Font,
                Texture2D Background,
                SpriteBatch spriteBatch
            ) gameAssets
        )
        {
            view = viewName;
            spriteFont = gameAssets.Font;

            MapManager = new MapManager(gameAssets.MapTextures);
            EnemyManager = new EnemyManager(gameAssets.EnemyTextures);
            Player = new Player(gameAssets.PlayerTexture);
            WeaponManager = new WeaponManager(gameAssets.WeaponTextures);
            HUDManager = new HUDManager(gameAssets.HUDTextures, gameAssets.Font);
            CombatHandler = new CombatHandler();
        }

        public override void Update(GameTime gameTime)
        {
            if (isGameOver)
                return;

            MapManager.Update(Player);
            EnemyManager.Update(Player);
            Player.Update();

            WeaponManager.Update(
                Player.position,
                Player.direction,
                Player.attackDirection,
                EnemyManager.Enemies
            );

            playerInEncounter =
                MapHandler.mapName == DataStructure._constants.Maps.EasyEncounter.Name;

            if (playerInEncounter)
            {
                HUDManager.Update();
                CombatHandler.Update(EnemyManager.Enemies, Player);
            }
        }

        public override void Draw()
        {
            if (isGameOver)
                DisplayGameOverMessage(Player.isDead);

            MapManager.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            WeaponManager.Draw(
                spriteBatch,
                Player.position,
                Player.attackIsCoolingDown,
                Player.attackDirection
            );

            if (playerInEncounter)
                HUDManager.Draw(spriteBatch, Player);
        }

        // Here we assume that Remove affects The enemies and their manager
        public override void Remove()
        {
            // These fields are static so we need to remove them too
            EnemyManager = null;
            EnemyManager.Enemies = null;

            HUDManager = null;

            CombatHandler.ResetCombatParameters();

            WeaponManager = null;
            WeaponManager.playerHoldsWeapon = false;
            WeaponManager.directionX = null;
            WeaponManager.layerDepth = 0.1f;
            WeaponManager.rotation = 0;

            isGameOver = false;
        }

        private void DisplayGameOverMessage(bool playerIsDead)
        {
            string text = "You win :D!";

            if (playerIsDead)
                text = "You lose :C!";

            spriteBatch.DrawString(
                spriteFont,
                text,
                new Vector2(settingsData.WindowWidth / 2, settingsData.WindowHeight / 2),
                Color.Black,
                0,
                spriteFont.MeasureString(text) / 2,
                scale: 1.0f,
                SpriteEffects.None,
                1f
            );
        }
    }
}
