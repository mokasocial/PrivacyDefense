using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree
{
    /// <summary>
    /// Logic for the player.
    /// </summary>
    class Player
    {
        /// <summary>
        /// The number of lives of left.
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// The player's score.
        /// </summary>
        public int Score { get; set; }

        public int Moneys { get; set; }
        /// <summary>
        /// Returns true if the player has lost.
        /// </summary>
        public bool HasLost
        {
            get
            {
                return Lives > 0;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Player()
        {
            Lives = 20;
            Score = 0;
            Moneys = 50;
        }
    }
}