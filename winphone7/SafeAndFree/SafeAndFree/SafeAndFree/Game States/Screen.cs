using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SafeAndFree
{
    public enum Screens
    {
        TITLE,
        GAME,
        WIN,
        LOSE
    }

    public abstract class Screen
    {
        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}