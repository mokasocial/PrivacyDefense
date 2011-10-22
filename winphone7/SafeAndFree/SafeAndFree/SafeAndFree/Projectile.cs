using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;
using Microsoft.Xna.Framework;
using SafeAndFree.Helpers;

namespace SafeAndFree
{
    public class Projectile : Actor
    {
        public WeaponStats Stats;
        public Vector2 CurrentPoint;
        public Creep TargetCreep;
        public Projectile(WeaponStats stats, Creep targetCreep, Vector2 startPoint)
        {
            Stats = stats;
            TargetCreep = targetCreep;
            CurrentPoint = startPoint;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Whether the projectile hit, and thus should be removed</returns>
        public bool Tick()
        {
            bool result;
            CurrentPoint = Calculator.MovementTowardsPoint(CurrentPoint, TargetCreep.Position, Stats.Speed, out result);
            return result;
        }
    }
}
