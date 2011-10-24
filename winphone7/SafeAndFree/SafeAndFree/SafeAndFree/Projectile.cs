using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;
using Microsoft.Xna.Framework;
using SafeAndFree.Helpers;
using SafeAndFree.Enumerations;

namespace SafeAndFree
{
    public class Projectile : Actor
    {
        public ProjectileTypes Type { get; private set; }
        public WeaponStats Stats;
        public Creep TargetCreep;

        public ProjectileTypes type;

        private int numFrames = 0;
        public int Frame = 0;
        public int CurrentDelay = 0;

        public float Rotation = 0;

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
                return new Vector2(CenterPosition.X - (int)ProjectileDefinitions.ProjectileStats[type].Width / 2, CenterPosition.Y - (int)ProjectileDefinitions.ProjectileStats[type].Height / 2);
            }
        }

        public Rectangle AnimationSource
        {
            get
            {
                return new Rectangle(Frame * ProjectileDefinitions.ProjectileStats[type].Width, 0, ProjectileDefinitions.ProjectileStats[type].Width, ProjectileDefinitions.ProjectileStats[type].Height);
            }
        }

        public Projectile(WeaponStats stats, Creep targetCreep, Vector2 startPoint, TowerTypes parentTowerType)
        {
            Stats = stats.GetCopy();
            TargetCreep = targetCreep;

            CenterPosition = startPoint;

            this.type = SelectTypeBasedOnTowerType(parentTowerType);
            this.TextureID =  TowerFactory.GetProjectileMediaID(type);

            this.numFrames = ProjectileDefinitions.ProjectileStats[type].NumFrames;
        }

        private ProjectileTypes SelectTypeBasedOnTowerType(TowerTypes type)
        {
            if (type == TowerTypes.Judge)
            {
                return ProjectileTypes.Gavel;
            }
            else if (type == TowerTypes.Lawyer)
            {
                return ProjectileTypes.Scroll;
            }

            return ProjectileTypes.Teacher;
            //have a switch here at some point
        }

        /// <summary>
        /// Update this Projectile instance.
        /// </summary>
        /// <returns>True if the projectile should be removed.</returns>
        public bool Update()
        {
            bool result;

            if (TargetCreep == null)
            {
                return true;
            }

            CenterPosition = Calculator.MovementTowardsPoint(CenterPosition, TargetCreep.CenterPosition, Stats.Speed, out result);

            // Don't hard code this!
            if (ProjectileTypes.Scroll == this.type)
            {
                // Math.Atan2 gives the angle between two points in RADIANS. We need to convert to degrees.
                this.Rotation = Calculator.ToDegrees((float)Math.Atan2(TargetCreep.CenterPosition.Y - this.CenterPosition.Y, TargetCreep.CenterPosition.X - this.CenterPosition.X));
            }
            if (result)
            {
                TargetCreep.TakeHit(this);
            }
            //if (Calculator.GetDistance(this.CenterPosition, TargetCreep.CenterPosition) <= Math.Abs(ProjectileDefinitions.ProjectileStats[type].Width/2 - TargetCreep.GetStat(CreepStats.Width)/2))
            //{
            //    result = TargetCreep.TakeHit(this);
            //}

            if (numFrames > 1)
            {
                if (--CurrentDelay <= 0)
                {
                    if (++Frame >= numFrames)
                    {
                        numFrames = 0;
                    }
                }
            }

            return result;
        }
    }
}
