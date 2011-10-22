using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SafeAndFree
{
    /// <summary>
    /// 
    /// </summary>
    abstract class Actor
    {
        public bool IsDead { get; protected set; }

        public Vector2 Position;

        public virtual void Update()
        {
        }
    }
}