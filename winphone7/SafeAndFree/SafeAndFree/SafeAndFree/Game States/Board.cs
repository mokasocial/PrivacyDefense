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
using SafeAndFree.Enumerations;
using SafeAndFree.Helpers;

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

        public static Vector2 TileDimensions
        {
            get; private set;
        }

        public static Vector2 TileCenter;

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
                    // Creep was killed.
                    creeps.RemoveAt(i--);
                }
                else if (creeps[i].Update(this.paths))
                {
                    // Creep reached the end.
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
            // Draw all creeps.
            foreach (Creep c in creeps)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(c.TextureID), c.Position, Color.White);
            }
        }

        /// <summary>
        /// Load the map information from xml.
        /// </summary>
        private void LoadMap()
        {
            Texture2D mapDefinition = TextureLibrary.GetTexture(MEDIA_ID.MAP_0);

            Color[] bits = new Color[mapDefinition.Width * mapDefinition.Height];
            mapDefinition.GetData<Color>(bits);

            for (int i = 0; i < mapDefinition.Height; i++)
            {
                for (int j = 0; j < mapDefinition.Width; j++)
                {
                    tileTextureIds[j, i] = bits[i * mapDefinition.Width + j].R;
                }
            }

            // TODO: Blit to a bitmap here, so that we don't draw
            // a bitmap for each tile on the draw loop.
            mapTiles = new Tile[30, 50];

            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    mapTiles[i, j] = new Tile(new Vector2(j * Board.TileDimensions.X, i * Board.TileDimensions.Y), (MEDIA_ID)tileTextureIds[j, i]);
                }
            }
        }

        /// <summary>
        /// Load the paths and waypoints information from xml.
        /// </summary>
        private void LoadPaths()
        {
            XmlReader reader = XmlReader.Create("MapDefinitions.xml");

            int lastPath = -1;
            int lastWaypoint = 0;

            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;

                if (nodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("map"))
                    {
                        TileDimensions = new Vector2(Int32.Parse(reader.GetAttribute("tileWidth")), Int32.Parse(reader.GetAttribute("tileHeight")));
                        TileCenter = new Vector2((int)(TileDimensions.X / 2), (int)(TileDimensions.Y / 2));
                    }
                    else if (reader.Name.Equals("paths"))
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
                        paths[lastPath][lastWaypoint++] = new Vector2(Int32.Parse(reader.GetAttribute("column")) * Board.TileDimensions.X + Board.TileCenter.X, Int32.Parse(reader.GetAttribute("row")) * Board.TileDimensions.Y + Board.TileCenter.Y);
                    }
                }
            }
        }

        /// <summary>
        /// Load creeps.
        /// </summary>
        private void LoadCreeps()
        {
            // Hard-coded for now.
            creeps = new List<Creep>();
            Dictionary<CreepStats, int> basicStats = new Dictionary<CreepStats, int>();
            basicStats.Add(CreepStats.Health, 50);
            basicStats.Add(CreepStats.Speed, 3);
            basicStats.Add(CreepStats.DamageToPlayer, 1);

            creeps.Add(new Creep(basicStats, new Vector2(paths[0][0].X, paths[0][0].Y), MEDIA_ID.CREEP_0, 0, 0));
        }
        ///
        ///Test load a tower
        ///
        private void LoadATowerTest()
        {
            Tower aTower = TowerFactory.GetTower(TowerTypes.Normal);
        }
    }
}