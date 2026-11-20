using System;

namespace SensorSystem
{
    public sealed class LegacySmokeDetector
    {
        private int smokeLevel;

        public LegacySmokeDetector(int smokeLevel)
        {
            this.smokeLevel = smokeLevel;
        }

        public int GetSmokeLevel() => smokeLevel; 
        public string GetDescription() => "Legacy smoke detector";

        public void SetSmokeLevel(int value)
        {
            smokeLevel = Math.Clamp(value, 0, 1000);
        }
    }
}