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
        /// Get the distance between two points.
        /// </summary>
        /// <param name="pos1">The first point.</param>
        /// <param name="pos2">The second point.</param>
        /// <returns>The distance between two points.</returns>
        public static double GetDistance(Vector2 pos1, Vector2 pos2)
        {
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }

        /// <summary>
        /// Gives the movement vector for an object to move towards
        /// the given point.
        /// </summary>
        /// <param name="start">The position to move from.</param>
        /// <param name="end">The position to move to.</param>
        /// <param name="speed">The vector's magnitude.</param>
        /// <param name="Connected">True if the vector </param>
        /// <returns>The movement vector.</returns>
        public static Vector2 MovementTowardsPoint(Vector2 start, Vector2 end, int speed, out bool Connected)
        {
            double totalDistance = GetDistance(start, end);
            double movementPercent = (double)speed * totalDistance;

            // Has the object connected with its destination?
            if (movementPercent >= 1)
            {
                Connected = true;
                return end;
            }

            // It has not connected.
            Connected = false;

            // Determine the movement vector.
            double xDiff = start.X - end.X;
            double yDiff = start.Y - end.Y;

            // Give it back.
            return new Vector2((float)(start.X + (xDiff * movementPercent)), (float)(start.Y + (yDiff * movementPercent)));
        }

    }
}