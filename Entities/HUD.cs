using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Managers;
using static SlipThrough2.Constants;

namespace SlipThrough2.Entities
{
    public class HUD
    {
        private static List<Texture2D> textures;
        public static List<Texture2D> healthBarTextures = new();
        public static List<Texture2D> manaBarTextures = new();
        private readonly SpriteFont Font;
        public static int[] healthBarTilePattern;
        public static int[] manaBarTilePattern;

        public HUD(List<Texture2D> HUDTexture, SpriteFont font, Player player)
        {
            textures = HUDTexture;
            Font = font;

            // Create data for player's health bar
            string color = "Red potion";
            healthBarTilePattern = PrepareDataForBars(player, color);

            // Mana bar
            color = "Blue potion";
            manaBarTilePattern = PrepareDataForBars(player, color);
        }

        public void Update() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the string
            string displayText = $"You have entered the door {MapManager.doorNumber}!";
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

        private static int[] PrepareDataForBars(Player player, string potionColor)
        {
            string empty = "Empty potion";
            int maxStat = potionColor == "Red potion" ? player.maxHealth : player.maxMana;
            int stat = potionColor == "Red potion" ? player.health : player.mana;
            int[] bar = new int[maxStat];

            // Fill the array with correct values
            for (int i = 0; i < stat; i++)
                bar[i] = POTIONS[potionColor].Index;

            // Fill the rest with empty potion if needed
            for (int i = stat; i < maxStat; i++)
                bar[i] = POTIONS[empty].Index;

            return bar;
        }

        public static void BuildTexturesForBars()
        {
            foreach (int index in healthBarTilePattern)
                healthBarTextures.Add(textures[index]);

            foreach (int index in manaBarTilePattern)
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
                        // Bottle is 10 px wide, sprite has 16 px 10/16 = 0.625
                        (int)(position.X + i * CELL_SIZE * 0.625f),
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
