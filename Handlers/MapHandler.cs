using System.Collections.Generic;
using System.Linq;
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

                    string[] specialTilesToCoverPlayer = { "Wo8v1", "Wo2v1" };

                    // If contains then 0.9 means at the top of all else other than the HUD, othervise 0 - underneath all else
                    float actualLayerDepth = specialTilesToCoverPlayer.Contains(tileCode) ? 0.9f : 0;

                    spriteBatch.Draw(
                        texture: tileTexture,
                        destinationRectangle: new Rectangle(
                            x: x * settingsData.CellSize,
                            y: y * settingsData.CellSize,
                            width: settingsData.CellSize,
                            height: settingsData.CellSize
                        ),
                        sourceRectangle: null,
                        color: Color.White,
                        rotation: 0,
                        origin: new Vector2(0, 0),
                        effects: SpriteEffects.None,
                        layerDepth: actualLayerDepth
                    );
                }
            }
        }
    }
}
