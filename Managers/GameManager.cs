using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
                List<Texture2D> WeaponTextures,
                SpriteFont Font,
                Texture2D Background,
                SpriteBatch spriteBatch
            ) gameAssets,
            List<SoundEffect> soundEffects
        )
        {
            ViewManager.LoadData(gameAssets);
            AudioManager.LoadSoundEffects(soundEffects);
        }

        public void Update(Game1 game1, GameTime gameTime)
        {
            ViewManager.Update(gameTime);
            KeyManager.Update(game1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.FrontToBack // The default is "Deffered", it ignores provided layerDepth
            );

            ViewManager.Draw();

            spriteBatch.End();
        }
    }
}
