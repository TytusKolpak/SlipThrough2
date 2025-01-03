﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Managers;

namespace SlipThrough2
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager gameManager;

        public Game1()
        {
            // Json data related pre-computations at the very beginning
            DataStructure.LoadJsonData();
            Settings settingsData = DataStructure._constants.Settings;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = settingsData.WindowWidth,
                PreferredBackBufferHeight = settingsData.WindowHeight
            };
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() => base.Initialize();

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Tiles data = DataStructure._constants.Tiles;

            Texture2D playerTexture = Content.Load<Texture2D>(data.PlayerPath);

            List<Texture2D> HUDTextures = new();

            foreach (var potionTile in data.Potion)
                HUDTextures.Add(Content.Load<Texture2D>(potionTile.Path));

            List<Texture2D> enemyTextures = new();
            foreach (var enemyTile in data.Enemy)
                enemyTextures.Add(Content.Load<Texture2D>(enemyTile.Path));

            List<Texture2D> mapTextures = new();
            foreach (var floorTile in data.Floor)
                mapTextures.Add(Content.Load<Texture2D>(floorTile.Path));

            List<Texture2D> weaponTextures = new();
            foreach (var weaponTile in data.Weapon)
                weaponTextures.Add(Content.Load<Texture2D>(weaponTile.Path));

            SpriteFont font = Content.Load<SpriteFont>("Font1");

            // All this hardcoded elements should be put into Data.json
            List<SoundEffect> soundEffect =
                new()
                {
                    Content.Load<SoundEffect>("doorOpening"),
                    Content.Load<SoundEffect>("walkingOnSand"),
                    Content.Load<SoundEffect>("splat"),
                    Content.Load<SoundEffect>("swordAttack"),
                };

            List<Texture2D> backgroundTextures =
                new()
                {
                    Content.Load<Texture2D>("sky"),
                    Content.Load<Texture2D>("far-clouds"),
                    Content.Load<Texture2D>("near-clouds"),
                    Content.Load<Texture2D>("far-mountains"),
                    Content.Load<Texture2D>("mountains"),
                    Content.Load<Texture2D>("trees"),
                };

            var gameAssets = (
                PlayerTextureAsset: playerTexture,
                EnemyTexturesAsset: enemyTextures,
                MapTexturesAsset: mapTextures,
                HUDTexturesAsset: HUDTextures,
                WeaponTextures: weaponTextures,
                FontAsset: font,
                BackgroundTexturesAsset: backgroundTextures,
                SpriteBatch: _spriteBatch
            );

            gameManager = new GameManager(gameAssets, soundEffect);
        }

        protected override void Update(GameTime gameTime)
        {
            gameManager.Update(this, gameTime);
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
