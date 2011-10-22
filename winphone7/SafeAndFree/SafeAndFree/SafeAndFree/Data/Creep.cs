using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Enumerations;
using Microsoft.Xna.Framework;
using SafeAndFree.Helpers;

namespace SafeAndFree.Data
{
    public class Creep
    {
        public Creep(Dictionary<CreepStats, int> stats, Vector2 currentPoint, Vector2 targetPoint)
        {
            CurrentPoint = currentPoint;
            TargetPoint = targetPoint;
            Stats = stats;
        }
        public Vector2 CurrentPoint { get; set; }
        public Vector2 TargetPoint { get; set; }
        public Dictionary<CreepStats, int> Stats { get; private set; }
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
        public void UpdateMovement()
        {
            Calculator.MovementTowardsPoint(CurrentPoint, TargetPoint, Stats[CreepStats.Speed]);   
        }
    }
}
