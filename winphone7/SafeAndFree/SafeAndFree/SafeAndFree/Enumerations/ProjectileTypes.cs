using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Enumerations
{
    public enum ProjectileTypes
    {
        Gavel, // Judge
        Scroll, // Lawyer
        Teacher // Teacher
    }

    public struct ProjectileTypeData
    {
        public int AnimationDelay;
        public int NumFrames;
        public int Width;
        public int Height;
    }

    public abstract class ProjectileDefinitions
    {
        public static Dictionary<ProjectileTypes, ProjectileTypeData> ProjectileStats;

        static ProjectileDefinitions()
        {
            ProjectileStats = new Dictionary<ProjectileTypes, ProjectileTypeData>();
        }
    }
}
