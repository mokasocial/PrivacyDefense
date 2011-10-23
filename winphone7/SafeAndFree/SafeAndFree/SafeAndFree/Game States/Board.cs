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
        private Player CurrentPlayer;
        private WaveManager waveManager;

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
            CurrentPlayer = new Player();
            projectileManager = new ProjectileManager();
            LoadResources();
        }

        private void LoadResources()
        {
            LoadMap();
            LoadData();
            LoadATowerTest();
        }

        /// <summary>
        /// The update loop.
        /// </summary>
        public void Update()
        {
            if (null != waveManager)
            {
                if (!waveManager.GameWon && waveManager.Update(creeps.Count == 0))
                {
                    creeps.Add(new Creep(CreepDefinitions.CreepStats[(CreepType)waveManager.waves[waveManager.currentWave][waveManager.nextSpawnIndex - 1][0]], new Vector2(paths[0][0].X, paths[0][0].Y), (MEDIA_ID)waveManager.waves[waveManager.currentWave][waveManager.nextSpawnIndex - 1][0], 0, 0));
                }
            }

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
                    // Getting multiple touch presses.
                    // To select and unselect by tapping, you
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
                t.Update();
                Creep target;
                if(t.CanFire && Calculator.BestShootableCreep(creeps, t.Position, t.GetTowerStats().Range, out target))
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

            spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.MENU_LEFT), new Rectangle(0, 0, 60, 480), Color.White);
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
        /// Load the paths, waypoints and wave data from xml.
        /// </summary>
        private void LoadData()
        {
            // Get the XML file with our data.
            XmlReader reader = XmlReader.Create("MapDefinitions.xml");

            // Initialize creep data.
            creeps = new List<Creep>();

            // Initialize path data.
            int lastPath = -1;
            int lastWaypoint = 0;

            // Initialize wave data.
            waveManager = new WaveManager();
            int[][][] waves = null;
            int lastWave = -1;
            int lastCreep = -1;

            // Initialize creep stat data.

            int lastCreepDefinition = 0;

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
                    else if (reader.Name.Equals("creepDefinition"))
                    {
                        CreepDefinitions.CreepStats.Add((CreepType)lastCreepDefinition++, new CreepTypeData 
                        { 
                            Width = Int32.Parse(reader.GetAttribute("width")),
                            Height = Int32.Parse(reader.GetAttribute("height")),
                            Health = Int32.Parse(reader.GetAttribute("health")),
                            Speed = Int32.Parse(reader.GetAttribute("speed")),
                            DamageToPlayer = Int32.Parse(reader.GetAttribute("damageToPlayer"))
                        });
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
                    else if (reader.Name.Equals("waves"))
                    {
                        waves = new int[Int32.Parse(reader.GetAttribute("numWaves"))][][];
                        lastWave = -1;
                    }
                    else if (reader.Name.Equals("wave"))
                    {
                        waves[++lastWave] = new int[Int32.Parse(reader.GetAttribute("numCreeps"))][];
                        lastCreep = -1;
                    }
                    else if (reader.Name.Equals("creep"))
                    {
                        waves[lastWave][++lastCreep] = new int[] { Int32.Parse(reader.GetAttribute("type")), Int32.Parse(reader.GetAttribute("delay")) };
                    }
                }
            }

            if (null != waves)
            {
                waveManager.SetWaves(waves);
            }
        }

        public void BuyPlaceTower(Vector2 location, TowerTypes type)
        {
            if(CurrentPlayer.WithdrawalMoney(TowerFactory.GetTowerCost(type))
            {
                towers.Add(TowerFactory.GetTower(type, location));
            }


        }
        ///
        /// Test load a tower
        ///
        private void LoadATowerTest()
        {
            towers = new List<Tower>();
            towers.Add(TowerFactory.GetTower(TowerTypes.Slow, new Vector2(70, 400)));
            towers.Add(TowerFactory.GetTower(TowerTypes.Normal, new Vector2(200, 300)));
            towers.Add(TowerFactory.GetTower(TowerTypes.Fast, new Vector2(200, 400)));
        }
    }
}