using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Enumerations;

namespace SafeAndFree.Data
{
    public class Debuff
    {
        public Debuff(CreepStats theTarget, int theAmount, int theDuration)
            : this(theTarget, theAmount, theDuration, false)
        {
            Target = theTarget;
            Amount = theAmount;
            Duration = theDuration;
        }
        public Debuff(CreepStats theTarget, int theAmount, int theDuration, bool isInstance)
        {
            Target = theTarget;
            Amount = theAmount;
            Duration = theDuration;
            IsCreepInstance = isInstance;
        }
        public static Debuff GetInstance(CreepStats theTarget, int theAmount, int theDuration)
        {
            return new Debuff(theTarget, theAmount, theDuration, true);
        }
        public void Update()
        {
            if (IsCreepInstance)
            {
                Duration--;
            }
        }
        public bool IsCreepInstance
        {
            get;
            private set;
        }
        public CreepStats Target { get; private set; }
        public int Amount { get; private set; }
        public int Duration { get; private set; }
    }
}
