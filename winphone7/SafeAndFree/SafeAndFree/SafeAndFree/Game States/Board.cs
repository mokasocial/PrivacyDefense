using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SafeAndFree.Data;

namespace SafeAndFree
{
    class Board : Screen
    {
        private Tile[,] mapTiles;

        public Board()
        {
            Texture2D mapDefinition = TextureLibrary.GetTexture(MEDIA_ID.MAP_0);

            Color[] bits = new Color[mapDefinition.Width * mapDefinition.Height];
            mapDefinition.GetData<Color>(bits);

            for (int i = 0; i < mapDefinition.Height; i++)
            {
                for (int j = 0; j < mapDefinition.Width; j++)
                {
                    if (bits[i * mapDefinition.Width + j].R == 0)
                    {
                        tileTextureIds[j, i] = 3;
                    }
                    else
                    {
                        tileTextureIds[j, i] = 0;
                    }
                }
            }

            mapTiles = new Tile[30, 50];

            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    mapTiles[i, j] = new Tile(new Vector2(j * 16, i * 16), (MEDIA_ID)tileTextureIds[j, i]);
                }
            }
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(mapTiles[i, j].TextureID), mapTiles[i, j].Position, Color.White);
                }
            }
        }

        // This array was put at the end because it's so long... and hard-coded.
        private int[,] tileTextureIds = new int[50, 30];
    }
}
