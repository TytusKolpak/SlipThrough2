using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        public GameManager(
            (
                Texture2D PlayerTexture,
                List<Texture2D> EnemyTextures,
                List<Texture2D> MapTextures,
                List<Texture2D> HUDTextures,
                SpriteFont Font,
                SpriteBatch spriteBatch
            ) gameAssets, List<SoundEffect> soundEffects
        )
        {
            ViewManager.LoadData(gameAssets);
            AudioManager.LoadSoundEffects(soundEffects);
        }

        public void Update(Game1 game1)
        {
            ViewManager.Update();
            KeyManager.Update(game1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            ViewManager.Draw();

            spriteBatch.End();
        }
    }
}
