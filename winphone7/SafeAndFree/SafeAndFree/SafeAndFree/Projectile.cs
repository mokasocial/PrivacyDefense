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
        public Vector2 CurrentPoint;
        public Creep TargetCreep;

        public Projectile(WeaponStats stats, Creep targetCreep, Vector2 startPoint, TowerTypes parentTowerType)
        {
            Stats = stats;
            TargetCreep = targetCreep;
            CurrentPoint = startPoint;
            SelectTypeBasedOnTowerType(parentTowerType);
            this.TextureID =  TowerFactory.GetProjectileMediaID(Type);
        }

        private void SelectTypeBasedOnTowerType(TowerTypes type)
        {
            Type = ProjectileTypes.Normal;
            //have a switch here at some point
        }

        /// <summary>
        /// Update this Projectile instance.
        /// </summary>
        /// <returns>True if the projectile should be removed.</returns>
        public bool Update()
        {
            bool result;
            CurrentPoint = Calculator.MovementTowardsPoint(CurrentPoint, TargetCreep.Position, Stats.Speed, out result);
            return result;
        }
    }
}
