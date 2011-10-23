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
using SafeAndFree.Game_States;
using Microsoft.Xna.Framework.Input.Touch;

namespace SafeAndFree
{
    /// <summary>
    /// Logic for the game board.
    /// </summary>
    public class Board : Screen
    {
        private ProjectileManager projectileManager;
        /// <summary>
        /// The grid of tiles.
        /// </summary>
        private Tile[,] mapTiles;

        /// <summary>
        /// List of towers on the map.
        /// </summary>
        private List<Creep> creeps;

        /// <summary>
        /// List of towers on the map.
        /// </summary>
        private List<Tower> towers;

        /// <summary>
        /// A set of paths that creeps can follow.
        /// The first rank are paths,
        /// the second rank are waypoints.
        /// </summary>
        private Vector2[][] paths;

        /// <summary>
        /// The consistent size of tiles.
        /// </summary>
        public static Vector2 TileDimensions { get; private set; }

        /// <summary>
        /// The offset (X and Y) from a tile's top left position
        /// that will give you the tile's center position.
        /// </summary>
        public static Vector2 TileCenter;

        public Vector2 selectedTile = new Vector2(-1, -1);

        /// <summary>
        /// Constructor.
        /// </summary>
        public Board()
        {
            projectileManager = new ProjectileManager();
            LoadResources();
        }

        private void LoadResources()
        {
            LoadMap();
            LoadPaths();
            LoadCreeps();
            LoadATowerTest();
        }

        /// <summary>
        /// The update loop.
        /// </summary>
        public void Update()
        {
            HandleCreepLoop();
            HandleTowerLoop();
            HandleProjectileLoop();
            HandleInput();
        }

        protected void HandleInput()
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchCollection)
            {
                int col = (int)Math.Floor(touchLocation.Position.X / Board.TileDimensions.X);
                int row = (int)Math.Floor(touchLocation.Position.Y / Board.TileDimensions.Y);

                if (selectedTile.X == col && selectedTile.Y == row)
                {
                    //selectedTile.X = -1;
                    //selectedTile.Y = -1;
                }
                else
                {
                    selectedTile.X = col;
                    selectedTile.Y = row;
                }
            }
        }

        protected void HandleProjectileLoop()
        {
            projectileManager.Update();
        }

        protected void HandleCreepLoop()
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

        protected void HandleTowerLoop()
        {
            foreach (Tower t in towers)
            {
                Creep target;
                if(Calculator.BestShootableCreep(creeps, t.Position, t.GetTowerStats().Range, out target))
                {
                    var proj = TowerFactory.GetTowerProjectile(t, target);
                    projectileManager.AddProjectile(proj);
                }
            }
        }

        /// <summary>
        /// Called every draw loop from the GameEngine.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.MAP_0), new Vector2(0, 0), Color.White);

            if (selectedTile.X >= 0 && selectedTile.Y >= 0)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.TILE_SELCT), new Vector2(selectedTile.X * TileDimensions.X, selectedTile.Y * TileDimensions.Y), Color.White);
            }

            // Draw all creeps.
            foreach (Creep c in creeps)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(c.TextureID), c.Position, Color.White);
            }

            foreach (Tower t in towers)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(t.TextureID), t.Position, Color.White);
            }

            foreach (Projectile p in projectileManager.Projectiles)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(p.TextureID), p.CurrentPoint, Color.White) ;
            }
        }

        /// <summary>
        /// Load the map information from xml.
        /// </summary>
        private void LoadMap()
        {
            mapTiles = new Tile[30, 50];

            for (int i = 0; i <= mapTiles.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mapTiles.GetUpperBound(1); j++)
                {
                    mapTiles[i, j] = new Tile(new Vector2(j * Board.TileDimensions.X, i * Board.TileDimensions.Y));
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
        /// Test load a tower
        ///
        private void LoadATowerTest()
        {
            towers = new List<Tower>();
            towers.Add(TowerFactory.GetTower(TowerTypes.Normal, new Vector2(50, 400)));
        }
    }
}