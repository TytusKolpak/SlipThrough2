using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class HUDManager
    {
        private static List<Texture2D> textures;
        public static List<Texture2D> healthBarTextures = new(),
            manaBarTextures = new();
        private readonly SpriteFont Font;
        public static int[] healthBarTilePattern,
            manaBarTilePattern;
        public static int iteration; // For "time"keeping
        private static List<Tile> data;
        private static Settings settingsData;

        public HUDManager(List<Texture2D> HUDTexture, SpriteFont font, Player player)
        {
            data = DataStructure._constants.Tiles.Potion;

            settingsData = DataStructure._constants.Settings;

            textures = HUDTexture;
            Font = font;

            // Create data for player's health bar
            string color = "Red potion";
            healthBarTilePattern = PrepareDataForBars(player, color);

            // Mana bar
            color = "Blue potion";
            manaBarTilePattern = PrepareDataForBars(player, color);

            iteration = 0;
        }

        public void Update()
        {
            if (iteration < 3 * 60)
                iteration++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the string for the first 3s
            if (iteration < 3 * 60)
            {
                string displayText = $"You have entered the door {MapManager.doorNumber}!";
                DisplayText(
                    spriteBatch,
                    displayText,
                    new(settingsData.WindowWidth / 2, settingsData.FontSize * 2),
                    centered: true
                );
            }

            // Icons as potions to display health level
            DisplayBar(
                spriteBatch,
                healthBarTextures,
                position: new(settingsData.CellSize * 0.5f, settingsData.CellSize * 0.5f)
            );
            DisplayBar(
                spriteBatch,
                manaBarTextures,
                position: new(settingsData.CellSize * 0.5f, settingsData.CellSize * 1.5f)
            );
        }

        private static int[] PrepareDataForBars(Player player, string potionColor)
        {
            string empty = "Empty potion";
            int maxStat = potionColor == "Red potion" ? player.maxHealth : player.maxMana;
            int stat = potionColor == "Red potion" ? player.health : player.mana;
            int[] bar = new int[maxStat];

            // Fill the array with correct values
            int index = data.FindIndex(potion => potion.Name == potionColor);
            for (int i = 0; i < stat; i++)
                bar[i] = index;

            // Fill the rest with empty potion if needed
            index = data.FindIndex(potion => potion.Name == empty);
            for (int i = stat; i < maxStat; i++)
                bar[i] = index;

            return bar;
        }

        public static void BuildTexturesForBars()
        {
            if (healthBarTextures.Count > 0)
                return; // No need to create add them, since they are already there

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
                        (int)(position.X + i * settingsData.CellSize * 0.625f),
                        (int)position.Y,
                        settingsData.CellSize,
                        settingsData.CellSize
                    ),
                    Color.White
                );
            }
        }
    }
}
