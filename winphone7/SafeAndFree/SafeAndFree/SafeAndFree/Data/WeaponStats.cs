using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Data
{
    public struct WeaponStats
    {
        public int Damage { get; private set; }
        public int Speed { get; private set; }
        public Debuff Gift { get; private set; }
        public int Splash { get; private set; }

        public WeaponStats(int damage, int speed, int splash)
            : this(damage, speed, splash, null)
        {
        }

        public WeaponStats(int damage, int speed, int splash, Debuff gift) : this()
        {
            Damage = damage;
            Speed = speed;
            Splash = splash;
            Gift = gift;
        }
        public WeaponStats GetCopy()
        {
            if (Gift != null)
            {
                return new WeaponStats(Damage, Speed, Splash, Debuff.GetInstance(Gift.Target, Gift.Amount, Gift.Duration));
            }
            else
            {
                return new WeaponStats(Damage, Speed, Splash);
            }
        }
    }
}
