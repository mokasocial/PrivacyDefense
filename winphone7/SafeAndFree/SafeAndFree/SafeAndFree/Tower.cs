using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;
using Microsoft.Xna.Framework;
using SafeAndFree.Enumerations;

namespace SafeAndFree
{
    /// <summary>
    /// Logic for tower objects.
    /// </summary>
    public class Tower : Actor
    {
        public TowerTypes Type { get; private set; }
        public int NextFire { get; private set; }
        public bool CanFire
        { get { return NextFire == 0; } }
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
        public Tower(TowerStats[] tStats, WeaponStats[] wStats, MEDIA_ID textureID, Vector2 startPosition, TowerTypes type)
        {
            towerStats = tStats;
            weaponStats = wStats;
            Level = 0;
            NextFire = 0;
            this._position = startPosition;
            Type = type;
            TextureID = textureID;
        }

        public TowerStats GetTowerStats()
        {
            return towerStats[Level];
        }

        public WeaponStats GetWeaponStats()
        {
            return weaponStats[Level];
        }

        public WeaponStats Fire()
        {
            NextFire += towerStats[Level].Delay;
            return GetWeaponStats();  
        }
        public void Update()
        {
            if (NextFire > 0)
            {
                NextFire--;
            }
        }
        public void GetLevelInfo(out int currAtk, out int currRange, out int currAtkDelay, out int cost,
            out int nextAtk, out int nextRange, out int nextAtkDelay)
        {
            currRange = towerStats[Level].Range;
            currAtkDelay = towerStats[Level].Delay;
            currAtk = weaponStats[Level].Damage;
            cost = towerStats[Level].CostToNext; 
            if (cost != -1)
            {
                nextRange = -1;
                nextAtk = -1;
                nextAtkDelay = -1;
            }
            else
            {
                nextRange = towerStats[Level + 1].Range;
                nextAtk = weaponStats[Level + 1].Damage;
                nextAtkDelay = towerStats[Level + 1].Delay;
            }
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
