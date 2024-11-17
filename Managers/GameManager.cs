using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        private ViewManager ViewManager;

        public GameManager(
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                SpriteFont Font
            ) gameAssets
        )
        {
            ViewManager = new ViewManager(gameAssets);
        }

        public void Update()
        {
            ViewManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            ViewManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
