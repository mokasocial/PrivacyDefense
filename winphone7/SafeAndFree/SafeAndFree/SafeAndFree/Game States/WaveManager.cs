using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree
{
    class WaveManager
    {
        public int[][][] waves;
        public int currentWave { get; private set; }
        public int nextSpawnIndex { get; private set; }

        private int delay;

        public bool GameWon { get; private set; }

        public WaveManager()
        {
            
        }

        public void SetWaves(int[][][] newWaves)
        {
            this.waves = newWaves;
            currentWave = 0;
            nextSpawnIndex = 0;
        }

        public bool Update()
        {
            if (--delay <= 0)
            {
                if (++nextSpawnIndex > waves.GetUpperBound(1))
                {
                    if (++currentWave > waves.GetUpperBound(0))
                    {
                        // Game won!
                        GameWon = true;

                        return false;
                    }

                    nextSpawnIndex = 0;

                    // Next delay + 5 seconds (@ 30 FPS).
                    delay = waves[currentWave][nextSpawnIndex][1] + 150;
                }

                return true;
            }

            return false;
        }
    }
}