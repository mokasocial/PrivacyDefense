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

        public bool Update(bool canStartNewWave)
        {
            if (--delay <= 0)
            {
                if (nextSpawnIndex + 1 > waves[currentWave].Length)
                {
                    if (!canStartNewWave)
                    {
                        return false;
                    }
                    else if (currentWave + 1 >= waves.Length)
                    {
                        // Game won!
                        GameWon = true;

                        return false;
                    }
                    else 
                    {
                        nextSpawnIndex = 0;
                        currentWave++;

                        // Next delay + 1.5 seconds (@ 30 FPS).
                        delay = waves[currentWave][nextSpawnIndex][1] * 6 + 45;
                    }
                }
                else
                {
                    delay = waves[currentWave][nextSpawnIndex][1] * 6;

                    nextSpawnIndex++;

                    return true;
                }
            }

            return false;
        }
    }
}