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
        private Dictionary<Vector2, Tower> towers;

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
            LoadData();
            LoadMap();
            LoadATowerTest();
        }

        /// <summary>
        /// The update loop.
        /// </summary>
        public override void Update()
        {
            if (null != waveManager)
            {
                //if (waveManager.Update(creeps.Count == 0))
                //{
                //    creeps.Add(new Creep(CreepDefinitions.ProjectileStats[(CreepType)waveManager.waves[waveManager.currentWave][waveManager.nextSpawnIndex - 1][0]], new Vector2(paths[0][0].X, paths[0][0].Y), (MEDIA_ID)waveManager.waves[waveManager.currentWave][waveManager.nextSpawnIndex - 1][0], 0, 0));
                //    if (waveManager.GameWon)
                //    {
                //        GameEngine.RunningEngine.Load(Screens.WIN);
                //        return;
                //    }
                //}
                if (waveManager.InfiniteUpdate(creeps.Count == 0))
                {
                    double boost = Math.Pow(1.2, waveManager.BonusWave);

                    CreepTypeData creepNormal = new CreepTypeData(){DamageToPlayer = 1, Health = (int)boost * 5, Height = 16, Width = 16, Speed = 3};
                    CreepTypeData creepFast = new CreepTypeData(){DamageToPlayer = 1, Health =(int)boost * 4, Height = 32, Width = 32, Speed = 5};
                    CreepTypeData creepVariant = new  CreepTypeData(){DamageToPlayer = 1, Health = (int)boost * 5, Height = 32, Width = 32, Speed = 4};
                    CreepTypeData creepBoss = new CreepTypeData(){DamageToPlayer = 2, Health = (int)boost * 25, Height = 32, Width = 32, Speed = 3};
                    
                    // What is this?
                    int thing = waveManager.BonusWave % 4;
                    Creep newCreep;

                    switch (thing)
                    {
                        case 0:
                            newCreep = new Creep(creepNormal, new Vector2(paths[0][0].X, paths[0][0].Y), MEDIA_ID.CREEP_0);
                            break;
                        case 1:
                             newCreep = new Creep(creepFast, new Vector2(paths[0][0].X, paths[0][0].Y), MEDIA_ID.CREEP_1);
                            break;
                        case 2:
                            newCreep = new Creep(creepVariant, new Vector2(paths[0][0].X, paths[0][0].Y), MEDIA_ID.CREEP_2);
                            break;
                        default:
                            newCreep = new Creep(creepBoss, new Vector2(paths[0][0].X, paths[0][0].Y), MEDIA_ID.CREEP_3);
                            break;
                    }

                    creeps.Add(newCreep);
                }
            }

            HandleCreepLoop();
            HandleTowerLoop();
            HandleProjectileLoop();
            HandleInput();
        }

        protected bool CheckButtonPress(Vector2 check)
        {
            if (check.X >= 10 && check.X <= 105)
            {
                if (check.Y >= 56 && check.Y < 145)
                {
                    BuyPlaceTower(TowerTypes.Judge);
                    return true;
                }
                else if (check.Y > 165 && check.Y <= 250)
                {
                    BuyPlaceTower(TowerTypes.Lawyer);
                    return true;
                }
                else if (check.Y > 270 && check.Y < 360)
                {
                    BuyPlaceTower(TowerTypes.Teacher);
                    return true;
                }
                else
                {
                    if (towers.ContainsKey(selectedTile))
                    {
                        UpdateTower(towers[selectedTile]);
                    }

                    return true;
                }
            }

            return false;
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

                    if (!CheckButtonPress(touchLocation.Position))
                    {
                      selectedTile.X = col;
                      selectedTile.Y = row;
                    }
                }
            }
        }

        protected void HandleProjectileLoop()
        {
            projectileManager.Update();
        }

        public void HandleClick(TowerTypes towerType) 
        {
            BuyPlaceTower(towerType);
        }

        protected void HandleCreepLoop()
        {
            for (int i = 0; i < creeps.Count; i++)
            {
                if (creeps[i].IsDead)
                {
                    // Creep was killed.creeps[i
                    
                    CurrentPlayer.AddScore(creeps[i].ScoreValue);
                    creeps.RemoveAt(i--);
                    CurrentPlayer.AddMoney(waveManager.BonusWave + 1);
                }
                else if (creeps[i].Update(this.paths))
                {
                    // Creep reached the end.
                    creeps.RemoveAt(i--);
                    CurrentPlayer.LoseLife();
                    if (CurrentPlayer.HasLost)
                    {
                        GameEngine.RunningEngine.Load(Screens.LOSE);
                    }
                }
            }
        }

        protected void HandleTowerLoop()
        {
            foreach (Tower t in towers.Values)
            {
                t.Update();
                Creep target;
                if(t.CanFire && Calculator.BestShootableCreep(creeps, t.Position, t.GetTowerStats().Range, out target))
                {
                    var proj = TowerFactory.GetTowerProjectile(t, target);
                    target.DeathForecast += proj.Stats.Damage;
                    projectileManager.AddProjectile(proj);
                }
            }
        }

        /// <summary>
        /// Called every draw loop from the GameEngine.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.MAP_0), new Vector2(0, 0), Color.White);

            if (selectedTile.X >= 0 && selectedTile.Y >= 0)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.TILE_SELCT), new Vector2(selectedTile.X * TileDimensions.X, selectedTile.Y * TileDimensions.Y), Color.White);
            }

            // Draw all creeps.
            foreach (Creep c in creeps)
            {
                if (0 != c.Rotation)
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(c.TextureID), new Vector2(c.Position.X + (int)c.GetStat(CreepStats.Width) / 2, c.Position.Y + (int)c.GetStat(CreepStats.Height) / 2), new Rectangle(0, 0, c.GetStat(CreepStats.Width), c.GetStat(CreepStats.Height)), Color.White, Calculator.ToRadians(c.Rotation), new Vector2(c.GetStat(CreepStats.Width) / 2, c.GetStat(CreepStats.Height) / 2), 1, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(c.TextureID), c.Position, Color.White);
                }
            }

            foreach (Tower t in towers.Values)
            {
                spriteBatch.Draw(TextureLibrary.GetTexture(t.TextureID), t.Position, new Rectangle(t.Level * (int)TileDimensions.X, 0, (int)TileDimensions.X, (int)TileDimensions.Y), Color.White);
            }

            foreach (Projectile p in projectileManager.Projectiles)
            {
                if (0 != p.Rotation)
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(p.TextureID),
                        new Vector2(p.Position.X + (int)ProjectileDefinitions.ProjectileStats[p.type].Width / 2, p.Position.Y + (int)ProjectileDefinitions.ProjectileStats[p.type].Height / 2),
                        new Rectangle(0, 0, ProjectileDefinitions.ProjectileStats[p.type].Width, ProjectileDefinitions.ProjectileStats[p.type].Height), 
                        Color.White, 
                        Calculator.ToRadians(p.Rotation),
                        new Vector2(ProjectileDefinitions.ProjectileStats[p.type].Width / 2, ProjectileDefinitions.ProjectileStats[p.type].Height / 2),
                        1, 
                        SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(p.TextureID), p.Position, p.AnimationSource, Color.White);
                }
            }

            spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.MENU_TOP), new Rectangle(0, 0, 800, 20), Color.White);
            spriteBatch.Draw(TextureLibrary.GetTexture(MEDIA_ID.MENU_LEFT), new Rectangle(0, 0, 144, 480), Color.White);

            // Lol, why use var?
            SpriteFont font = TextureLibrary.GetFont(FONT_ID.HUDINFO);
            spriteBatch.DrawString(font, "Level: " + waveManager.currentWave, new Vector2(125, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Cash: " + CurrentPlayer.Moneys, new Vector2(250, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Lives: " + CurrentPlayer.Lives, new Vector2(375, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Score: " + CurrentPlayer.Score, new Vector2(500, 0), Color.LightGreen);
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

            // Initialize projectile stat data.
            int lastProjectileDefinition = 0;

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
                        CreepTypeData creepData = new CreepTypeData
                        {
                            Width = Int32.Parse(reader.GetAttribute("width")),
                            Height = Int32.Parse(reader.GetAttribute("height")),
                            Health = Int32.Parse(reader.GetAttribute("health")),
                            Speed = Int32.Parse(reader.GetAttribute("speed")),
                            DamageToPlayer = Int32.Parse(reader.GetAttribute("damageToPlayer"))
                        };

                        CreepDefinitions.CreepStats.Add((CreepType)(lastCreepDefinition++), creepData);
                    }
                    else if (reader.Name.Equals("projectileDefinition"))
                    {
                        ProjectileTypeData projectileData = new ProjectileTypeData
                        {
                            AnimationDelay = Int32.Parse(reader.GetAttribute("delay")),
                            NumFrames = Int32.Parse(reader.GetAttribute("numFrames")),
                            Width = Int32.Parse(reader.GetAttribute("width")),
                            Height = Int32.Parse(reader.GetAttribute("height"))
                        };

                        ProjectileDefinitions.ProjectileStats.Add((ProjectileTypes)lastProjectileDefinition++, projectileData);
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

        public void BuyPlaceTower(TowerTypes type)
        {
            if(!towers.ContainsKey(selectedTile) && CurrentPlayer.WithdrawalMoney(TowerFactory.GetTowerCost(type)))
            {
                towers.Add(selectedTile, TowerFactory.GetTower(type, new Vector2(selectedTile.X * TileDimensions.X, selectedTile.Y * TileDimensions.Y)));
            }
        }

        private void UpdateTower(Tower theOneToOneUp)
        {
            if (theOneToOneUp.CanLevel && CurrentPlayer.WithdrawalMoney(theOneToOneUp.GetTowerStats().CostToNext))
            {
                theOneToOneUp.LevelUp();
            }
        }
        ///
        /// Test load a tower
        ///
        private void LoadATowerTest()
        {
            towers = new Dictionary<Vector2, Tower>();
            //towers.Add(selectedTile, TowerFactory.GetTower(TowerTypes.Teacher, new Vector2(70, 400)));
            //towers.Add(TowerFactory.GetTower(TowerTypes.Gavel, new Vector2(200, 300)));
            //towers.Add(TowerFactory.GetTower(TowerTypes.Lawyer, new Vector2(200, 400)));
        }
    }
}