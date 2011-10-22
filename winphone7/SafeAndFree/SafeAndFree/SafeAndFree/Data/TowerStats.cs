using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Data
{
    public struct TowerStats
    {
        public TowerStats(int delay, int range, int costToNext)
        {
            Delay = delay;
            Range = range;
            CostToNext = costToNext;
        }
        public int Delay { get; private set; }
        public int Range { get; private set; }
        public int CostToNext { get; private set; }
    }
}
