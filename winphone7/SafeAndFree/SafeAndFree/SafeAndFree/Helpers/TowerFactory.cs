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

        public static Tower GetTower(TowerTypes type, Vector2 position)
        {
            WeaponStats[] wStats;
            TowerStats[] tStats;
            GetStatsForTowerType(type, out tStats, out wStats);
            return new Tower(tStats, wStats, GetTowerMediaID(type), position, type);
            //switch (type)
            //{
            //    case TowerTypes.Gavel:
            //    case TowerTypes.Lawyer:
            //    case TowerTypes.Teacher:
            //    case TowerTypes.Splash:
            //}
        }

        public static int GetTowerCost(TowerTypes type)
        {
            switch(type)
            {
                case TowerTypes.Lawyer:
                    return 15;                       
                case TowerTypes.Teacher:
                    return 20;
                default:
                    return 10;//Gavel tower
            }
        }
        public static void GetStatsForTowerType(TowerTypes type, out TowerStats[] towerStats, out WeaponStats[] weaponStats)
        {
            switch (type)
            {
                case TowerTypes.Lawyer://Base cost 15 min delay is 4
                    towerStats = new TowerStats[7] {new TowerStats(15, 150, 25), new TowerStats(12, 160, 50), new TowerStats(9, 170, 90), 
                         new TowerStats(7, 180, 200), new TowerStats(6, 190, 450),new TowerStats(5, 195, 700),new TowerStats(4, 200, -1) };
                    weaponStats = new WeaponStats[7] { new WeaponStats(5, 10, 1), new WeaponStats(18, 10, 1), new WeaponStats(40, 10, 1), 
                         new WeaponStats(120, 10, 1), new WeaponStats(500, 10, 1),new WeaponStats(1500, 10, 1),new WeaponStats(3000, 10, 1) };
                break;
                //case TowerTypes.Splash:
                case TowerTypes.Teacher://base 20 
                    Debuff slow = new Debuff(CreepStats.Speed, 1, 10);
                    Debuff slow2 = new Debuff(CreepStats.Speed, 2, 20);
                towerStats = new TowerStats[5] { new TowerStats(70, 140, 40), new TowerStats(50, 150, 60), 
                    new TowerStats(30, 150, 100), new TowerStats(20, 160, 200), new TowerStats(10, 170, -1) };
                weaponStats = new WeaponStats[5] { new WeaponStats(20, 6, 1, slow), new WeaponStats(40, 6, 1, slow), 
                    new WeaponStats(80, 6, 1, slow), new WeaponStats(200, 6, 1, slow2), new WeaponStats(500, 6, 1, slow2) };
                break;
                //case TowerTypes.Gavel:

                default: //normal for now (judge) Base 10;
                towerStats = new TowerStats[6] { new TowerStats(13, 160, 30), new TowerStats(12, 170, 50), new TowerStats(10, 190, 100), 
                    new TowerStats(10, 210, 180), new TowerStats(9, 230, 400), new TowerStats(8, 250, -1) };
                weaponStats = new WeaponStats[6] { new WeaponStats(5, 10, 1), new WeaponStats(20, 10, 1), new WeaponStats(50, 10, 1), 
                    new WeaponStats(150, 10, 1), new WeaponStats(700, 10, 1), new WeaponStats(2000, 10, 1) };
                break;
            }
        }

        /// <summary>
        /// Should eventually return an array, for different levels
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MEDIA_ID GetTowerMediaID(TowerTypes type)
        {
            if (type == TowerTypes.Judge)
            {
                return MEDIA_ID.TOWER_0;
            }
            else if (type == TowerTypes.Lawyer)
            {
                return MEDIA_ID.TOWER_1;
            }

            return MEDIA_ID.TOWER_2;
            //switch(type) <-needs that eventually
        }

        public static MEDIA_ID GetProjectileMediaID(ProjectileTypes type)
        {
            switch(type)
            {
                case ProjectileTypes.Teacher:
                    return MEDIA_ID.PROJECTILE_2;
                case ProjectileTypes.Gavel:
                    return MEDIA_ID.PROJECTILE_0;
                default:
                    return MEDIA_ID.PROJECTILE_1;
             }
        }

        public static Projectile GetTowerProjectile(Tower tower, Creep target)
        {
            return new Projectile(tower.Fire(), target, tower.Position, tower.Type);
        }
    }
}
