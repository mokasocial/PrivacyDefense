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
        public int Lives { get; private set; }

        /// <summary>
        /// The player's score.
        /// </summary>
        public int Score { get; private set; }

        public int Moneys { get; private set; }
        /// <summary>
        /// Returns true if the player has lost.
        /// </summary>
        public bool HasLost
        {
            get
            {
                return Lives <= 0;
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

        public void AddScore(int bonus)
        {
            Score += bonus;
        }
        public void AddMoney(int dollars)
        {
            Moneys += dollars;
        }
        public void LoseLife()
        {
            Lives--;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yen"></param>
        /// <returns>Whether the transation worked(they had enough dalla)</returns>
        public bool WithdrawalMoney(int yen)
        {
            if (yen <= Moneys)
            {
                Moneys -= yen;
                return true;
            }
            return false;

        }
    }
}