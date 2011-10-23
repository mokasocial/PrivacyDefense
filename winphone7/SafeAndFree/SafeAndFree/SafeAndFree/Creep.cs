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
        /// <summary>
        /// The center position of this instance.
        /// </summary>
        private Vector2 CenterPosition;

        public bool IsDead
        {
            get { return Stats[CreepStats.Health] <= 0; }
        }
        /// <summary>
        /// The index of the path to follow.
        /// </summary>
        private int _path = 0;

        /// <summary>
        /// The index of the waypoint this instance
        /// is walking towards.
        /// </summary>
        private int _nextWaypoint = 0;

        /// <summary>
        /// Get the top-left position of this instance.
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return new Vector2(CenterPosition.X - Board.TileCenter.X, CenterPosition.Y - Board.TileCenter.Y);
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

        /// <summary>
        /// A reference to the stats of this creep.
        /// </summary>
        public Dictionary<CreepStats, int> Stats { get; private set; }

        /// <summary>
        /// Creep instance constructor.
        /// </summary>
        /// <param name="stats">A dictionary of stats for this instance.</param>
        /// <param name="position">The spawning position.</param>
        /// <param name="textureID">The MEDIA_ID for this creep's texture.</param>
        public Creep(Dictionary<CreepStats, int> stats, Vector2 position, MEDIA_ID textureID)
        {
            this.CenterPosition = new Vector2(position.X + Board.TileCenter.X, position.Y + Board.TileCenter.Y);
            this.TextureID = textureID;

            this.Stats = stats;
        }

        /// <summary>
        /// Creep instance constructor.
        /// </summary>
        /// <param name="stats">A dictionary of stats for this instance.</param>
        /// <param name="position">The spawning position.</param>
        /// <param name="textureID">The MEDIA_ID for this creep's texture.</param>
        /// <param name="path">The index of the path to follow.</param>
        /// <param name="startingWaypoint">The waypoint to spawn at.</param>
        public Creep(Dictionary<CreepStats, int> stats, Vector2 position, MEDIA_ID textureID, int path, int startingWaypoint)
            : this(stats, position, textureID)
        {
            this._path = path;
            this._nextWaypoint = startingWaypoint;
        }

        /// <summary>
        /// Update this Creep instance.
        /// </summary>
        /// <param name="paths">A set of paths that we may follow.</param>
        /// <returns></returns>
        public bool Update(Vector2[][] paths)
        {

            Vector2[] ourPath = paths[_path];

            if (Math.Abs(this.CenterPosition.X - ourPath[this._nextWaypoint].X) > this.Stats[CreepStats.Speed])
            {
                if (ourPath[this._nextWaypoint].X > this.CenterPosition.X)
                {
                    this.CenterPosition.X += this.Stats[CreepStats.Speed];
                }
                else if (ourPath[this._nextWaypoint].X < this.CenterPosition.X)
                {
                    this.CenterPosition.X -= this.Stats[CreepStats.Speed];
                }
            }
            else if (Math.Abs(this.CenterPosition.Y - ourPath[this._nextWaypoint].Y) > this.Stats[CreepStats.Speed])
            {
                if (ourPath[this._nextWaypoint].Y > this.CenterPosition.Y)
                {
                    this.CenterPosition.Y += this.Stats[CreepStats.Speed];
                }
                else if (ourPath[this._nextWaypoint].Y < this.CenterPosition.Y)
                {
                    this.CenterPosition.Y -= this.Stats[CreepStats.Speed];
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
            Stats[CreepStats.Health] -= bullet.Stats.Damage;
            if (bullet.Stats.Gift != null)
            {

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
        public int? GetStat(CreepStats key)
        {
            if (Stats.ContainsKey(key))
            {
                return Stats[key];
            }
            else
            {
                return null;
            }
        }
    }
}
