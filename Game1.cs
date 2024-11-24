﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Managers;
using static SlipThrough2.Constants;

namespace SlipThrough2
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WINDOW_WIDTH,
                PreferredBackBufferHeight = WINDOW_HEIGHT
            };
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D playerTexture = Content.Load<Texture2D>(PLAYER_TILE_PATH);

            List<Texture2D> HUDTextures = new();
            foreach (var potion in POTIONS)
                HUDTextures.Add(Content.Load<Texture2D>(potion.Value.TilePath));

            List<Texture2D> enemyTextures = new();
            foreach (string tilePath in ENEMY_TILE_PATHS)
                enemyTextures.Add(Content.Load<Texture2D>(tilePath));

            List<Texture2D> mapTextures = new();
            foreach (string tilePath in TILE_PATHS)
                mapTextures.Add(Content.Load<Texture2D>(tilePath));

            SpriteFont font = Content.Load<SpriteFont>("Font1");

            var gameAssets = (
                PlayerTextureAsset: playerTexture,
                EnemyTexturesAsset: enemyTextures,
                MapTexturesAsset: mapTextures,
                HUDTexturesAsset: HUDTextures,
                FontAsset: font,
                SpriteBatch: _spriteBatch
            );

            gameManager = new GameManager(gameAssets);
        }

        protected override void Update(GameTime gameTime)
        {
            gameManager.Update(this);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color backgroundColor = new(118, 59, 54);
            GraphicsDevice.Clear(backgroundColor);
            gameManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
