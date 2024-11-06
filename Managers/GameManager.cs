using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SlipThrough2.Entities;
using System.Collections.Generic;
using SlipThrough2.World;

namespace SlipThrough2.Managers
{
    public class GameManager
    {
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public Map Map { get; private set; }

        public GameManager(Texture2D playerTexture, List<Texture2D> enemyTextures, List<Texture2D> mapTextures)
        {
            Map = new Map(mapTextures);

            Player = new Player(playerTexture);

            Enemies = new List<Enemy>();
            foreach (var texture in enemyTextures)
            {
                Enemies.Add(new Enemy(texture));
            }
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);

            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Map.Draw(spriteBatch);

            Player.Draw(spriteBatch);
            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            spriteBatch.End();

        }
    }
}
