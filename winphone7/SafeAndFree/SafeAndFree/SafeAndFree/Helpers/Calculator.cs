using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SafeAndFree.Helpers
{
    /// <summary>
    /// Generic match helper methods.
    /// </summary>
    static class Calculator
    {
        /// <summary>
        /// TODO: Is this necessary?
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static double GetDistance(Vector2 pos1, Vector2 pos2)
        {
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }


        public static Vector2 MovementTowardsPoint(Vector2 start, Vector2 end, Vector2 nextTarget, int speed)
        {
            double totalDistance = GetDistance(start, end);
            double movementPercent = (double)speed * totalDistance;
            double xDiff = start.X - end.X;
            double yDiff = start.Y - end.Y;
            return new Vector2((float)(start.X + (xDiff * movementPercent)), (float)(start.Y + (yDiff * movementPercent)));
        }

    }
}
