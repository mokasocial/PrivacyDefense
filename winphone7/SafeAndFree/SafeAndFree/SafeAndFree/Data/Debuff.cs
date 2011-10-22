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
        {
            Target = theTarget;
            Amount = theAmount;
            Duration = theDuration;
        }
        public CreepStats Target { get; set; }
        public int Amount { get; set; }
        public int Duration { get; set; }
    }
}
