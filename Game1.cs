using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlipThrough2.Entities;
using SlipThrough2.Managers;
using System.Collections.Generic;

namespace SlipThrough2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private GameManager gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            Texture2D playerTexture = Content.Load<Texture2D>("Tiles/tile_0096");

            List<Texture2D> enemyTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>("Tiles/tile_0123"),
                Content.Load<Texture2D>("Tiles/tile_0124")
            };

            List<Texture2D> mapTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>("Tiles/tile_0000"), // 0 blank ground
                Content.Load<Texture2D>("Tiles/tile_0012"), // 1 ground v2
                Content.Load<Texture2D>("Tiles/tile_0024"), // 2 ground v3
                Content.Load<Texture2D>("Tiles/tile_0005"), // 3 right upper corner
                Content.Load<Texture2D>("Tiles/tile_0017"), // 4 right bottom corner
                Content.Load<Texture2D>("Tiles/tile_0016"), // 5 left bottom corner
                Content.Load<Texture2D>("Tiles/tile_0004"), // 6 left upper corner
                Content.Load<Texture2D>("Tiles/tile_0015"), // 7 left wall
                Content.Load<Texture2D>("Tiles/tile_0013"), // 8 right wall
                Content.Load<Texture2D>("Tiles/tile_0002"), // 9 bottom wall
                Content.Load<Texture2D>("Tiles/tile_0026"), // 10 upper wall
            };

            gameManager = new GameManager(playerTexture, enemyTextures, mapTextures);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            gameManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}