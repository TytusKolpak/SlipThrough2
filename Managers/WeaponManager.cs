using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;
using SlipThrough2.Handlers;

namespace SlipThrough2.Managers
{
    class WeaponManager
    {
        private readonly List<Tile> DATA = DataStructure._constants.Tiles.Weapon;
        private readonly int cellSize = DataStructure._constants.Settings.CellSize;
        private static readonly Maps mapsData = DataStructure._constants.Maps;
        private readonly List<Texture2D> TEXTURES;
        private int weaponIndex;
        public static Rectangle rectangle;
        private Point size,
            adjustedPlayerPosition;

        public static bool attachedToPlayer;
        public static float layerDepth = 0.1f,
            rotation;
        public static string direction;

        public WeaponManager(List<Texture2D> weaponTextures)
        {
            TEXTURES = weaponTextures;
            SetWeaponParameters(11, 10, "Sword");
        }

        public void Update() { }

        public void Draw(SpriteBatch batch, Vector2 playerPositon, Vector2 playerDirection)
        {
            if (MapHandler.mapName == mapsData.NewMain.Name)
            {
                // Overwrite the original weapon location - make player carry the weapon
                if (attachedToPlayer)
                {
                    // This code is pretty culnky. It works but do refactor later.
                    if (playerDirection.X == cellSize)
                        direction = "right";

                    if (playerDirection.X == -cellSize)
                        direction = "left";

                    if (direction == "right")
                    {
                        adjustedPlayerPosition = new(
                            (int)playerPositon.X + cellSize,
                            (int)playerPositon.Y
                        );
                        rotation = MathHelper.ToRadians(45);
                    }

                    if (direction == "left")
                    {
                        adjustedPlayerPosition = new(
                            (int)playerPositon.X - cellSize * 3 / 4,
                            (int)playerPositon.Y + cellSize * 3 / 4
                        );
                        rotation = MathHelper.ToRadians(-45);
                    }

                    rectangle = new Rectangle(adjustedPlayerPosition, size);
                    layerDepth = 0.6f; // Just above the player
                }

                batch.Draw(
                    texture: TEXTURES[weaponIndex],
                    destinationRectangle: rectangle,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: rotation,
                    origin: new Vector2(0, 0),
                    effects: SpriteEffects.None,
                    layerDepth: layerDepth // Above ground, below enemy
                );
            }
        }

        private void SetWeaponParameters(int x, int y, string name)
        {
            weaponIndex = DATA.FindIndex(element => element.Name == name);
            Point position = new(x * cellSize, y * cellSize);
            size = new(cellSize, cellSize);
            rectangle = new(position, size);
        }
    }
}
