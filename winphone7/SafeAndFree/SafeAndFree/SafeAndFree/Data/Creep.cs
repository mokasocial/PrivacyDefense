using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Enumerations;

namespace SafeAndFree.Data
{
    public class Creep
    {
        public Dictionary<CreepStats, int> Stats { get; private set; }
    }
}
