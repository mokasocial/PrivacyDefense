using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SafeAndFree.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Input;

namespace SafeAndFree
{
    /// <summary>
    /// Logic for the game board.
    /// </summary>
    class Board : Screen
    {
        /// <summary>
        /// TODO: This shouldn't be kept,
        /// this should be loaded from the map image
        /// and then represented in a single bitmap.
        /// </summary>
        private int[,] tileTextureIds = new int[50, 30];

        /// <summary>
        /// The grid of tiles.
        /// </summary>
        private Tile[,] mapTiles;

        /// <summary>
        /// List of active creeps.
        /// </summary>
        private List<Creep> creeps;

        /// <summary>
        /// A set of paths that creeps can follow.
        /// The first rank are paths,
        /// the second rank are waypoints.
        /// </summary>
        private Vector2[][] paths;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Board()
        {
            LoadMap();

            LoadPaths();

            LoadCreeps();
        }

        /// <summary>
        /// The update loop.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < creeps.Count; i++)
            {
                if (creeps[i].IsDead)
                {
                    creeps.RemoveAt(i--);
                }
                else if (creeps[i].Update(this.paths))
                {
                    creeps.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// Called every draw loop from the GameEngine.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all tiles.
            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(mapTiles[i, j].TextureID), mapTiles[i, j].Position, Color.White);
                }
            }

            // Draw all creeps.
            foreach (Creep c in creeps)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(c.TextureID), c.Position, Color.White);
            }
        }

        private void LoadMap()
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

            // TODO: Blit to a bitmap here, so that we don't draw
            // a bitmap for each tile on the draw loop.
            mapTiles = new Tile[30, 50];

            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    mapTiles[i, j] = new Tile(new Vector2(j * 16, i * 16), (MEDIA_ID)tileTextureIds[j, i]);
                }
            }
        }

        private void LoadPaths()
        {
            StreamResourceInfo definitionsStream = Application.GetResourceStream(new Uri("/SafeAndFree;component/MapDefinitions.xml", UriKind.RelativeOrAbsolute));
            XmlReader reader = XmlReader.Create(definitionsStream.Stream);

            int lastPath = -1;
            int lastWaypoint = 0;

            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;

                if (nodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("paths"))
                    {
                        paths = new Vector2[Int32.Parse(reader.GetAttribute("numPaths"))][];
                    }
                    else if (reader.Name.Equals("path"))
                    {
                        paths[++lastPath] = new Vector2[Int32.Parse(reader.GetAttribute("numWaypoints"))];
                        lastWaypoint = 0;
                    }
                    else if (reader.Name.Equals("waypoint"))
                    {
                        paths[lastPath][lastWaypoint++] = new Vector2(Int32.Parse(reader.GetAttribute("column")) * 16 + 8, Int32.Parse(reader.GetAttribute("row")) * 16 + 8);
                    }
                }
            }
        }

        private void LoadCreeps()
        {
            // Hard-coded for now.
            creeps = new List<Creep>();
            creeps.Add(new Creep(new Vector2(paths[0][0].X - 8, paths[0][0].Y - 8), MEDIA_ID.CREEP_0, 0, 0));
        }
    }
}
