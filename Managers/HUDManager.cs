using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Entities;

namespace SlipThrough2.Managers
{
    public class HUDManager
    {
        private static List<Tile> potionTiles = DataStructure._constants.Tiles.Potion;
        private static Settings settingsData = DataStructure._constants.Settings;
        private static List<Texture2D> textures;
        private readonly SpriteFont Font;
        public static int cellSize = settingsData.CellSize;

        public HUDManager(List<Texture2D> HUDTexture, SpriteFont font)
        {
            textures = HUDTexture;
            Font = font;
        }

        public void Update() { }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            DisplayEncounterData(spriteBatch);

            // Display health and mana levels
            Vector2 healthBarPosition = new(cellSize * 0.5f, cellSize * 0.5f);
            Vector2 manaBarPosition = new(cellSize * 0.5f, cellSize * 1.5f);
            DisplayPlayerStatBar(
                spriteBatch,
                "Red potion",
                healthBarPosition,
                player.health,
                player.maxHealth
            );
            DisplayPlayerStatBar(
                spriteBatch,
                "Blue potion",
                manaBarPosition,
                player.mana,
                player.maxMana
            );
        }

        private void DisplayPlayerStatBar(
            SpriteBatch sb,
            string fullPotion,
            Vector2 position,
            int stat,
            int maxStat
        )
        {
            for (int i = 0; i < maxStat; i++)
            {
                // Select the potion tile to draw
                string tile;
                if (i < stat)
                    tile = fullPotion;
                else
                    tile = "Empty potion";

                int tileIndex = potionTiles.FindIndex(element => element.Name == tile);

                // Potion bottle is 10 px wide, sprite has 16 px 10/16
                int potionWidth = cellSize * 10 / 16;

                // Move every other potion to the right
                Vector2 offset = new(potionWidth * i, 0);
                Point size = new(cellSize, cellSize);

                // Draw the potion(s)
                sb.Draw(
                    texture: textures[tileIndex],
                    destinationRectangle: new Rectangle((position + offset).ToPoint(), size),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: 0.99f
                );
            }
        }

        private void DisplayText(SpriteBatch sb, string text, Vector2 position, string justify)
        {
            Vector2 origin = justify switch
            {
                "left" => Vector2.Zero,
                "center" => Font.MeasureString(text) / 2,
                "right" => new(Font.MeasureString(text).X, 0),
                _ => throw new NotImplementedException()
            };

            sb.DrawString(
                Font,
                text,
                position,
                Color.Black,
                0,
                origin,
                scale: 0.6f,
                SpriteEffects.None,
                1f
            );
        }

        public static void DisplayEnemyHealthBar(
            SpriteBatch sb,
            Vector2 position,
            int health,
            int maxHealth
        )
        {
            for (int i = 0; i < maxHealth; i++)
            {
                // Determine what potion to draw
                string tile;
                if (i < health)
                    tile = "Red potion";
                else
                    tile = "Empty potion";

                int tileIndex = potionTiles.FindIndex(element => element.Name == tile);

                // Draw each next to the other, but make them centered
                int potionWidth = 10;
                int barWidth = maxHealth * potionWidth;
                int potionTileAdjuster = 3; // This is here due to how the potion tile was drawn by the artist
                int barOffset = -barWidth / 2 + cellSize / 2 - potionTileAdjuster;
                Vector2 offset = new(potionWidth * i + barOffset, cellSize);

                // There has to be special smaller size for this tile
                Point size = new(cellSize / 2, cellSize / 2);

                // Draw the potion(s)
                sb.Draw(
                    texture: textures[tileIndex],
                    destinationRectangle: new Rectangle((position + offset).ToPoint(), size),
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: 0.99f //Below HUD text
                );
            }
        }

        public void DisplayEncounterData(SpriteBatch spriteBatch)
        {
            string enemyDifficulty = MapManager.doorNumber switch
            {
                1 => "Easy",
                2 => "Medium",
                3 => "Hard",
                _ => throw new NotImplementedException()
            };

            string displayText =
                $"Door number: {MapManager.doorNumber}.\n"
                // + $"Encounter: {MapHandler.mapName}.\n"
                + $"Enemy: {enemyDifficulty}.";

            Vector2 encounterDataPosition =
                new(settingsData.WindowWidth, 0);
            DisplayText(spriteBatch, displayText, encounterDataPosition, "right");
        }
    }
}
