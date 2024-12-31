using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlipThrough2.Data;

namespace SlipThrough2.Views
{
    public class MovingBackground
    {
        // The texture to draw as a background.
        private List<Texture2D> backgroundTextures;
        private static Settings data = DataStructure._constants.Settings;
        public static float scrollingSpeed = 5;
        private int nrOfElements;
        private Vector2[] positions;
        private float[] scale;
        private int[] repetitions,
            width;

        public MovingBackground(List<Texture2D> BackgroundTextures)
        {
            backgroundTextures = BackgroundTextures;

            // To fit the background to the window
            nrOfElements = backgroundTextures.Count;
            scale = new float[nrOfElements]; // This should be the same for all imgs since they have the same height but in case of a change it can stay as array
            repetitions = new int[nrOfElements];
            width = new int[nrOfElements];
            positions = new Vector2[nrOfElements];

            for (int i = 0; i < nrOfElements; i++)
            {
                if (i == 0)
                    //The very first background image has to be stretched horizontally because it is not moving
                    scale[i] = (float)data.WindowWidth / backgroundTextures[i].Width;
                else
                    // This scale is for stretching the image (both horizontally and vertically) to fit the height of the window
                    scale[i] = (float)data.WindowHeight / backgroundTextures[i].Height;

                if (i == nrOfElements - 1)
                    // Put the last one a little lower
                    positions[i].Y = 50;

                width[i] = (int)(backgroundTextures[i].Width * scale[i]);

                // This scale is for repeating the image, but to fill window horizontally + one off screen
                float horizontalScale = (float)data.WindowWidth / width[i] + 1;
                repetitions[i] = (int)Math.Ceiling(horizontalScale); // Image will need to repeat 3 times if scale is 2.42

                // Start from the very right of the window (have this img fully outside of the window and slowly appearing inside)
                positions[i].X = data.WindowWidth;
            }
        }

        public void Update(float deltaX)
        {
            for (int i = 0; i < nrOfElements; i++)
            {
                // Keep moving the position of the background image at all time
                // artificial speed gradient using i, first img stationary
                positions[i].X -= deltaX * i;

                // When the most right (main) image will be all inside the window
                if (positions[i].X < data.WindowWidth - backgroundTextures[i].Width * scale[i])
                    // snap it to 1 its width to the right so that there are no empty window parts
                    positions[i].X = data.WindowWidth;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Cover all images forming the background
            for (int i = 0; i < nrOfElements; i++)
            {
                // Cover all necessary repetitions for this image
                for (int j = 0; j < repetitions[i]; j++)
                {
                    Vector2 actualPosition = positions[i];
                    actualPosition.X -= width[i] * j;
                    float layerDepth = (float)i / 100; // int/float = int (=Math.Floor(float))

                    spriteBatch.Draw(
                        texture: backgroundTextures[i],
                        position: actualPosition,
                        sourceRectangle: null,
                        color: Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: scale[i],
                        effects: SpriteEffects.None,
                        layerDepth: layerDepth
                    );
                }
            }
        }
    }
}
