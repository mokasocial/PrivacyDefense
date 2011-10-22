using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Data
{
    public struct WeaponStats
    {
        public WeaponStats(int damage, int speed, int splash, Debuff gift)
        {
            Damage = damage;
            Speed = speed;
            Splash = splash;
            Gift = gift;
        }
        public WeaponStats(int damage, int speed, int splash) : this(damage, speed, splash, null)
        {
        }

        public int Damage { get; private set; }
        public int Speed { get; private set; }
        public Debuff? Gift { get; private set; }
        public int Splash { get; private set; }
    }
}
