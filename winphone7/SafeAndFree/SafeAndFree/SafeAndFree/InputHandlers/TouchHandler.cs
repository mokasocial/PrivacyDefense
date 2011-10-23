using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace SafeAndFree.InputHandlers
{
    static class TouchHandler
    {
        private static TouchCollection touches;

        private static bool wasDown = false;
        public static bool JustClicked = false;
        /// <summary>
        /// Evaluates to true if the touches have started.
        /// </summary>
        public static bool IsDown
        {
            get
            {
                return touches.Count > 0;
            }
        }

        /// <summary>
        /// Evaluates to true if the touch just ended.
        /// </summary>
        public static bool IsClicked
        {
            get
            {
                return wasDown && !IsDown;
            }
        }

        /// <summary>
        /// Evaluates to true if the touch just started.
        /// </summary>
        public static bool IsPressed
        {
            get
            {
                return !wasDown && IsDown;
            }
        }

        public static void Update()
        {
            if (IsDown && !wasDown)//just got clicked!
            {
                JustClicked = true;
            }
            else 
            {
                JustClicked = false;
            }
            wasDown = IsDown;
            touches = TouchPanel.GetState();
        }
    }
}
