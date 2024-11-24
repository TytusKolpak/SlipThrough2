using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Handlers
{
    public class MapHandler
    {
        private static Settings settingsData;

        private readonly List<Texture2D> textures;

        // Of rooms in a map (and of tiles in a room)
        public static int[,][,] currentPattern;

        // Where player can move, what to enter and so on
        public static int[,] currentFunctionalPattern;

        // For easy tracking
        public static string mapName;

        public MapHandler(List<Texture2D> mapTextures)
        {
            settingsData = DataStructure._constants.Settings;
            textures = mapTextures;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Map is divided into a grid of rooms
            // Each room is divided into a grid of tiles
            // Draw each tile in a horizontal line of the room, then
            // Draw each horizontal line of the room, then
            // Draw each room in a horizontal line of the map, then
            // Draw each horizontal line of the grid in the map
            for (int column = 0; column < settingsData.ColumnCount; column++)
            {
                for (int row = 0; row < settingsData.RowCount; row++)
                {
                    for (int y = 0; y < settingsData.RoomSize; y++)
                    {
                        for (int x = 0; x < settingsData.RoomSize; x++)
                        {
                            int tileId = currentPattern[row, column][y, x];
                            Texture2D tileTexture = textures[tileId];
                            spriteBatch.Draw(
                                tileTexture,
                                new Rectangle(
                                    x * settingsData.CellSize
                                        + column * settingsData.RoomSize * settingsData.CellSize,
                                    y * settingsData.CellSize
                                        + row * settingsData.RoomSize * settingsData.CellSize,
                                    settingsData.CellSize,
                                    settingsData.CellSize
                                ),
                                Color.White
                            );
                        }
                    }
                }
            }
        }
    }
}
