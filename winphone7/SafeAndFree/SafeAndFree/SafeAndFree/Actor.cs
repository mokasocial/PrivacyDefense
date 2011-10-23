using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SafeAndFree.Data;

namespace SafeAndFree
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Actor
    {
        public bool IsDead { get; protected set; }

        protected Vector2 _position;
        /// <summary>
        /// The texture for this instance.
        /// </summary>
        public MEDIA_ID TextureID;

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