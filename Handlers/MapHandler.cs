using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Handlers
{
    public class MapHandler
    {
        private static Settings settingsData = DataStructure._constants.Settings;
        private static List<FloorTile> tileData = DataStructure._constants.Tiles.Floor;
        private readonly List<Texture2D> textures;
        public static string[,] currentTileLayout;
        public static int[,] currentFunctionalPattern;
        public static string mapName;

        public MapHandler(List<Texture2D> mapTextures) => textures = mapTextures;

        public void Draw(SpriteBatch spriteBatch)
        {
            // Go through a 2d array of strings and draw the tiles it represents
            for (int y = 0; y < settingsData.MapHeight; y++)
            {
                for (int x = 0; x < settingsData.MapWidth; x++)
                {
                    string tileCode = currentTileLayout[y, x]; // This is now "Wo8v1"
                    int index = tileData.FindIndex(tile => tile.Name == tileCode); // This is now 31
                    Texture2D tileTexture = textures[index]; // This is now an image

                    spriteBatch.Draw(
                        tileTexture,
                        new Rectangle(
                            x: x * settingsData.CellSize,
                            y: y * settingsData.CellSize,
                            width: settingsData.CellSize,
                            height: settingsData.CellSize
                        ),
                        Color.White
                    );
                }
            }
        }
    }
}
