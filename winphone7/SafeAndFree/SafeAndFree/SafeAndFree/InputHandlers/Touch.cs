using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace SafeAndFree.InputHandlers
{
    class Touch
    {
        private TouchCollection touches;

        private bool wasDown = false;

        /// <summary>
        /// Evaluates to true if the touches have started.
        /// </summary>
        public bool IsDown
        {
            get
            {
                return touches.Count > 0;
            }
        }

        /// <summary>
        /// Evaluates to true if the touch just ended.
        /// </summary>
        public bool IsClicked
        {
            get
            {
                return wasDown && !IsDown;
            }
        }

        /// <summary>
        /// Evaluates to true if the touch just started.
        /// </summary>
        public bool IsPressed
        {
            get
            {
                return !wasDown && IsDown;
            }
        }

        public void Update()
        {
            wasDown = IsDown;

            touches = TouchPanel.GetState();
        }
    }
}
