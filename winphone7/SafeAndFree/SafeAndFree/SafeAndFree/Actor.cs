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

        protected Vector2 _position;

        public virtual Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public virtual void Update()
        {
        }
    }
}