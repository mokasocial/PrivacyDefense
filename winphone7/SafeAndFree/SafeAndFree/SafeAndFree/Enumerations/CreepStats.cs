using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Enumerations
{
    public enum CreepStats
    {
        Speed,
        Health,
        DamageToPlayer,
        Width,
        Height
    }

    public enum CreepType
    {
        DataMiner,
        GovernmentSearcher,
        Defcon,
        Corporate
    }

    public struct CreepTypeData
    {
        public int Width;
        public int Height;
        public int Health;
        public int Speed;
        public int DamageToPlayer;
    }

    public abstract class CreepDefinitions
    {
        public static Dictionary<CreepType, CreepTypeData> CreepStats;

        static CreepDefinitions()
        { 
            CreepStats = new Dictionary<CreepType, CreepTypeData>();
        }
    }
}