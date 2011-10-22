using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;

namespace SafeAndFree
{
    public class Tower : Actor
    {
        public int NextFire { get; private set; }
        private TowerStats[] towerStats;
        private WeaponStats[] weaponStats;
        public int Level 
        { 
            get; 
            private set; 
        }
        public bool CanLevel 
        { 
            get 
            { 
                return Level < towerStats.Length && Level < weaponStats.Length && (towerStats[Level].CostToNext >= 0); 
            } 
        }
        public Tower(TowerStats[] tStats, WeaponStats[] wStats)
        {
            towerStats = tStats;
            weaponStats = wStats;
            Level = 0;
            NextFire = 0;
        }
        public TowerStats GetTowerStats()
        {
            return towerStats[Level];
        }
        public WeaponStats GetWeaponStats()
        {
            return weaponStats[Level];
        }
        public void LevelUp()
        {
            if(CanLevel)
            {
                Level++;
            }
        }

    }
}
