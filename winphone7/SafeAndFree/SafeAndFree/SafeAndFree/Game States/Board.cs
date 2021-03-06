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
using SafeAndFree.InputHandlers;

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
        private static Vector2[][] paths;

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

        // Doing work after? Fix this.
        private static bool hasLoaded = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Board()
        {
            CurrentPlayer = new Player();
            projectileManager = new ProjectileManager();

            InitInstanceData();

            if (!hasLoaded)
            {
                hasLoaded = true;
                LoadData();
            }
        }

        private void InitInstanceData()
        {
            // Initialize creep data.
            creeps = new List<Creep>();

            // Initialize wave data.
            waveManager = new WaveManager();

            towers = new Dictionary<Vector2, Tower>();
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
                    CreepTypeData creepBoss = new CreepTypeData(){DamageToPlayer = 2, Health = (int)boost * 40, Height = 32, Width = 32, Speed = 3};
                    
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
            if (check.X < 144)
            {
                if (TouchHandler.IsPressed)
                {
                    if (towers.ContainsKey(selectedTile) && check.Y >= 20 && check.Y < 110)
                    {
                        UpdateTower(towers[selectedTile]);
                    }
                    if (check.Y >= 20 && check.Y < 110)
                    {
                        BuyPlaceTower(TowerTypes.Teacher);
                    }
                    else if (check.Y >= 120 && check.Y < 210)
                    {
                        BuyPlaceTower(TowerTypes.Lawyer);
                        return true;
                    }
                    else if (check.Y >= 220 && check.Y < 310)
                    {
                        BuyPlaceTower(TowerTypes.Judge);
                    }
                }

                return true;
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
                else if (creeps[i].Update(paths))
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
                spriteBatch.Draw(TextureLibrary.GetTexture(t.TextureID), t.Position, new Rectangle((t.Level <= 4 ? t.Level : 4) * (int)TileDimensions.X, 0, (int)TileDimensions.X, (int)TileDimensions.Y), Color.White);
            }

            foreach (Projectile p in projectileManager.Projectiles)
            {
                if (0 != p.Rotation)
                {
                    spriteBatch.Draw(TextureLibrary.GetTexture(p.TextureID),
                        new Vector2(p.Position.X + (int)ProjectileDefinitions.ProjectileStats[p.type].Width / 2, p.Position.Y + (int)ProjectileDefinitions.ProjectileStats[p.type].Height / 2),
                        new Rectangle(0, 0, ProjectileDefinitions.ProjectileStats[p.type].Width+12, ProjectileDefinitions.ProjectileStats[p.type].Height+12), 
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

            SpriteFont font = TextureLibrary.GetFont(FONT_ID.HUDINFO);

            spriteBatch.DrawString(font, "Level: " + (1 + waveManager.BonusWave), new Vector2(125, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Cash: " + CurrentPlayer.Moneys, new Vector2(250, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Lives: " + CurrentPlayer.Lives, new Vector2(375, 0), Color.LightGreen);
            spriteBatch.DrawString(font, "Score: " + CurrentPlayer.Score, new Vector2(500, 0), Color.LightGreen);

            if (towers.ContainsKey(selectedTile))
            {
                spriteBatch.Draw(TextureLibrary.GetButtonTexture(BUTTON_MEDIA_ID.UPGRADE), new Rectangle(5, 20, 94, 90), Color.White);
                DrawUpgradeStuff(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(TextureLibrary.GetButtonTexture(BUTTON_MEDIA_ID.TEACHER), new Rectangle(5, 20, 94, 90), Color.White);
                spriteBatch.Draw(TextureLibrary.GetButtonTexture(BUTTON_MEDIA_ID.LAWYER), new Rectangle(5, 120, 94, 90), Color.White);
                spriteBatch.Draw(TextureLibrary.GetButtonTexture(BUTTON_MEDIA_ID.JUDGE), new Rectangle(5, 220, 94, 90), Color.White);
            }
        }

        private void DrawUpgradeStuff(SpriteBatch batch)
        {
            var font = TextureLibrary.GetFont(FONT_ID.HUDINFO);
            int cA, cR, cD, cC, nA, nR, nD;
            this.towers[selectedTile].GetLevelInfo(out cA, out cR, out cD, out cC, out nA, out nR, out nD);

            batch.DrawString(font, "Cost:  " , new Vector2(5, 120), Color.DarkGreen);
            batch.DrawString(font, (cC != -1 ? cC.ToString() : "None"), new Vector2(5, 140), Color.Red);
            batch.DrawString(font, "Attack: ", new Vector2(5, 160), Color.DarkGreen);
            batch.DrawString(font, cA + " ->" + (cC != -1 ? nA.ToString() : "None"), new Vector2(5, 180), Color.Red);
            batch.DrawString(font, "Range: " , new Vector2(5, 200), Color.DarkGreen);
            batch.DrawString(font, cR + " ->" + (cC != -1 ? nR.ToString() : "None"), new Vector2(5, 220), Color.Red);
            batch.DrawString(font, "Delay: " , new Vector2(5, 240), Color.DarkGreen);
            batch.DrawString(font,  cD + " ->" + (cC != -1 ? nD.ToString() : "None"), new Vector2(5, 260), Color.Red);

        }

        /// <summary>
        /// Load the paths, waypoints and wave data from xml.
        /// </summary>
        private void LoadData()
        {
            // Get the XML file with our data.
            XmlReader reader = XmlReader.Create("MapDefinitions.xml");

            // Initialize path data.
            int lastPath = -1;
            int lastWaypoint = 0;

            // Initialize wave data.
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

    }
}