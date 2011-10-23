using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;
using Microsoft.Xna.Framework;

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
        public Tower(TowerStats[] tStats, WeaponStats[] wStats, MEDIA_ID textureID, Vector2 startPosition)
        {
            towerStats = tStats;
            weaponStats = wStats;
            Level = 0;
            NextFire = 0;
            this._position = startPosition;
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

        public void Fire(out WeaponStats missile)
        {
            missile = weaponStats[Level];
            NextFire += towerStats[Level].Delay;
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
