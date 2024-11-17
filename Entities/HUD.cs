using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public class HUD
    {
        private readonly List<Texture2D> textures;
        private readonly List<Texture2D> healthBarTextures = new();
        private readonly List<Texture2D> manaBarTextures = new();
        private readonly SpriteFont Font;
        public Vector2 position = new(CELL_SIZE * 0.5f, WINDOW_HEIGHT - CELL_SIZE * 1.5f);

        public HUD(List<Texture2D> HUDTexture, SpriteFont font)
        {
            textures = HUDTexture;
            Font = font;
        }

        public void Update() { }

        public void Draw(SpriteBatch spriteBatch, MAP_NAME mapName)
        {
            // Draw the string
            string displayText = $"You have enetered the {mapName.ToString()}!";
            DisplayText(spriteBatch, displayText, new(WINDOW_WIDTH / 2, FONT_SIZE), centered: true);

            // Icons as potions to display health level
            DisplayBar(
                spriteBatch,
                healthBarTextures,
                position: new(CELL_SIZE * 0.5f, CELL_SIZE * 0.5f)
            );
            DisplayBar(
                spriteBatch,
                manaBarTextures,
                position: new(CELL_SIZE * 0.5f, CELL_SIZE * 1.5f)
            );
        }

        public void ArrangeTextures()
        {
            foreach (int index in HEALTH_HUD_TILE_PATTERN)
                healthBarTextures.Add(textures[index]);

            foreach (int index in MANA_HUD_TILE_PATTERN)
                manaBarTextures.Add(textures[index]);
        }

        private void DisplayText(SpriteBatch sb, string text, Vector2 position, bool centered)
        {
            sb.DrawString(
                Font,
                text,
                position,
                Color.Black,
                0,
                centered ? Font.MeasureString(text) / 2 : new(0, 0),
                1.0f,
                SpriteEffects.None,
                0.5f
            );
        }

        private static void DisplayBar(SpriteBatch sb, List<Texture2D> textures, Vector2 position)
        {
            for (int i = 0; i < textures.Count; i++)
            {
                sb.Draw(
                    textures[i],
                    new Rectangle(
                        (int)(position.X + i * CELL_SIZE * 0.625f), // Bottle is 10 px wide, sprite has 16 px 10/16 = 0.625
                        (int)position.Y,
                        CELL_SIZE,
                        CELL_SIZE
                    ),
                    Color.White
                );
            }
        }
    }
}
