using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        private readonly ViewManager ViewManager;

        public GameManager(
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
            ViewManager = new ViewManager(gameAssets);
        }

        public void Update(Game1 game1)
        {
            // Main Keys behavior
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game1.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                ViewManager.SwitchView(Constants.VIEW_NAME.MainGame);

            ViewManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            ViewManager.Draw();

            spriteBatch.End();
        }
    }
}
