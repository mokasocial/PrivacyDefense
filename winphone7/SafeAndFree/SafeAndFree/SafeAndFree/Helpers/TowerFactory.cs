using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Enumerations;
using SafeAndFree.Data;
using Microsoft.Xna.Framework;

namespace SafeAndFree.Helpers
{
    class TowerFactory
    {

        public static Tower GetTower(TowerTypes type)
        {
            WeaponStats[] wStats;
            TowerStats[] tStats;
            GetStatsForTowerType(type, out tStats, out wStats);
            return new Tower(tStats, wStats, GetTowerMediaID(type));
            //switch (type)
            //{
            //    case TowerTypes.Normal:
            //    case TowerTypes.Fast:
            //    case TowerTypes.Slow:
            //    case TowerTypes.Splash:
            //}
        }
        public static void GetStatsForTowerType(TowerTypes type, out TowerStats[] towerStats, out WeaponStats[] weaponStats)
        {
            //switch (type)
            //{            
            //    case TowerTypes.Fast:
            //    case TowerTypes.Slow:
            //    case TowerTypes.Splash:
            //    case TowerTypes.Normal:
            //    case default :    
            towerStats = new TowerStats[5] { new TowerStats(20, 5, 5), new TowerStats(15, 6, 10), new TowerStats(13, 7, 20), new TowerStats(11, 8, 40), new TowerStats(10, 9, -1) };
            weaponStats = new WeaponStats[5] { new WeaponStats(10, 10, 1), new WeaponStats(20, 10, 1), new WeaponStats(40, 10, 1), new WeaponStats(80, 10, 1), new WeaponStats(200, 10, 1) };
            //break;
            //}
        }
        /// <summary>
        /// Should eventually return an array, for different levels
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MEDIA_ID GetTowerMediaID(TowerTypes type)
        {
            return MEDIA_ID.TOWER_0;
            //switch(type) <-needs that eventually
        }
        public static Projectile GetTowerProjectile(Creep target, Tower tower, Vector2 startPoint)
        {
            return new Projectile(tower.GetWeaponStats(), target, startPoint);
        }
    }
}
