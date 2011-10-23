using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SafeAndFree.Data;
using SafeAndFree.Helpers;
using SafeAndFree.Enumerations;

namespace SafeAndFree
{
    public class Creep : Actor
    {
        private List<Debuff> CurrentDebuffs;



        public int ScoreValue { get; private set; }
        /// <summary>
        /// The index of the path to follow.
        /// </summary>
        private int _path = 0;
        public int DeathForecast = 0;
        /// <summary>
        /// The index of the waypoint this instance
        /// is walking towards.
        /// </summary>
        private int _nextWaypoint = 0;

        private Dictionary<CreepStats, int> stats;

        /// <summary>
        /// The center position of this instance.
        /// </summary>
        public Vector2 CenterPosition;

        /// <summary>
        /// Get the top-left position of this instance.
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return new Vector2(CenterPosition.X - (int)GetStat(CreepStats.Width) / 2, CenterPosition.Y - (int)GetStat(CreepStats.Height) / 2);
            }
        }
        
        /// <summary>
        /// The index of the path this creep travels.
        /// </summary>
        public int Path
        {
            get
            {
                return _path;
            }
        }

        public int DistanceTravelled { get; private set; }

        public new bool IsDead
        {
            get
            { 
                return Stats[CreepStats.Health] <= 0; 
            }
        }

        public float Rotation { get; private set; }

        /// <summary>
        /// A reference to the stats of this creep.
        /// </summary>
        public Dictionary<CreepStats, int> Stats 
        { 
            get 
            { 
                if(CurrentDebuffs.Count>0)
                {
                    Dictionary<CreepStats, int> newStats = new Dictionary<CreepStats, int>(); 
                    foreach(KeyValuePair<CreepStats, int> kvp in stats)
                    {
                        newStats.Add(kvp.Key, kvp.Value);
                    }
                    CurrentDebuffs.ForEach(cd => 
                    {
                        if(newStats.ContainsKey(cd.Target))
                        {
                            newStats[cd.Target] = Math.Min(newStats[cd.Target], stats[cd.Target] - cd.Amount);
                        }
                    });

                    return newStats;
                } 
                return stats;
            }
            private set { stats = value; }
        }

        /// <summary>
        /// Creep instance constructor.
        /// </summary>
        /// <param name="stats">A dictionary of stats for this instance.</param>
        /// <param name="position">The spawning position.</param>
        /// <param name="textureID">The MEDIA_ID for this creep's texture.</param>
        public Creep(CreepTypeData creepData, Vector2 position, MEDIA_ID textureID)
        {
            CurrentDebuffs = new List<Debuff>();
            this.CenterPosition = new Vector2(position.X , position.Y);
            this.TextureID = textureID;

            DistanceTravelled = 0;

            Rotation = 0;

            this.stats = new Dictionary<CreepStats, int>();
            this.stats.Add(CreepStats.Width, creepData.Width);
            this.stats.Add(CreepStats.Height, creepData.Height);
            this.stats.Add(CreepStats.Health, creepData.Health);
            this.stats.Add(CreepStats.DamageToPlayer, creepData.DamageToPlayer);
            this.stats.Add(CreepStats.Speed, creepData.Speed);
            this.ScoreValue = creepData.Health;
        }

        /// <summary>
        /// Creep instance constructor.
        /// </summary>
        /// <param name="stats">A dictionary of stats for this instance.</param>
        /// <param name="position">The spawning position.</param>
        /// <param name="textureID">The MEDIA_ID for this creep's texture.</param>
        /// <param name="path">The index of the path to follow.</param>
        /// <param name="startingWaypoint">The waypoint to spawn at.</param>
        public Creep(CreepTypeData creepData, Vector2 position, MEDIA_ID textureID, int path, int startingWaypoint)
            : this(creepData, position, textureID)
        {
            this._path = path;
            this._nextWaypoint = startingWaypoint;
        }

        private void UpdateDebuffs()
        {
            List<Debuff> retList = new List<Debuff>() ;
            CurrentDebuffs.ForEach(d => { d.Update(); if (d.Duration <= 0) { retList.Add(d); } });
            retList.ForEach(r => { CurrentDebuffs.Remove(r); });
        }
        /// <summary>
        /// Update this Creep instance.
        /// </summary>
        /// <param name="paths">A set of paths that we may follow.</param>
        /// <returns></returns>
        public bool Update(Vector2[][] paths)
        {
            UpdateDebuffs();

            Vector2[] ourPath = paths[_path];

            int moveDistance = this.Stats[CreepStats.Speed];
            DistanceTravelled += moveDistance;

            if (Math.Abs(this.CenterPosition.X - ourPath[this._nextWaypoint].X) > moveDistance)
            {
                if (ourPath[this._nextWaypoint].X > this.CenterPosition.X)
                {
                    this.CenterPosition.X += moveDistance;
                    this.Rotation = 0;
                }
                else if (ourPath[this._nextWaypoint].X < this.CenterPosition.X)
                {
                    this.CenterPosition.X -= moveDistance;
                    this.Rotation = 180;
                }
            }
            else if (Math.Abs(this.CenterPosition.Y - ourPath[this._nextWaypoint].Y) > moveDistance)
            {
                if (ourPath[this._nextWaypoint].Y > this.CenterPosition.Y)
                {
                    this.CenterPosition.Y += moveDistance;
                    this.Rotation = 90;
                }
                else if (ourPath[this._nextWaypoint].Y < this.CenterPosition.Y)
                {
                    this.CenterPosition.Y -= moveDistance;
                    this.Rotation = 270;
                }
            }
            else
            {
                this.CenterPosition.X = ourPath[this._nextWaypoint].X;
                this.CenterPosition.Y = ourPath[this._nextWaypoint].Y;

                this._nextWaypoint++;

                if (ourPath.Length == this._nextWaypoint)
                {
                    // We have reached the end.
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Makes the creep take a projectile hit.
        /// </summary>
        /// <param name="bullet">The projectile that hits the creep</param>
        /// <returns>True if the creep dies, false otherwise</returns>
        public bool TakeHit(Projectile bullet)
        {
            DeathForecast -= bullet.Stats.Damage;
            Stats[CreepStats.Health] -= bullet.Stats.Damage;
            if (bullet.Stats.Gift != null)
            {
                CurrentDebuffs.Add(bullet.Stats.GetCopy().Gift);
            }
            if (Stats[CreepStats.Health] <= 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get a CreepStat from this instance.
        /// </summary>
        /// <param name="key">The CreepStat to retrieve a value of.</param>
        /// <returns>The value of the given CreepStat or null if given an invalid key.</returns>
        public int GetStat(CreepStats key)
        {
            if (Stats.ContainsKey(key))
            {
                return Stats[key];
            }
            else
            {
                return -1;
            }
        }
    }
}
