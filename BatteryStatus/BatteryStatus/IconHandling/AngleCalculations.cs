//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.IconHandling.AngleCalculations.cs
// Created on: 20191029
//-----------------------------------------------

using BatteryStatus.Support;

namespace BatteryStatus.IconHandling
{
    /// <summary>
    /// Angle calculations for drawing.
    /// </summary>
    internal class AngleCalculations
    {
        private const float FullCircle    = 360.0F;
        private const float StartPoint    = 270.0F;

        private const int   ChargingSteps = 3;
        private int         _currentStep;
        private float       _percentage;

        private int CurrentStep
        {
            get => _currentStep;
            set => _currentStep = value > ChargingSteps ? 0 : value;
        }

        public float Percentage
        {
            get => _percentage;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _percentage = value;
                }
                else
                {
                    throw new PropertyOutOfRangeException();
                }
            }
        }

        public float Start => StartPoint;

        public float End => (FullCircle / 100) * Percentage;

        public float Start2 => Start + End;

        public float End2(bool showChargingAnimation)
        {
            if (showChargingAnimation) { CurrentStep++; } else { CurrentStep = 0; }
            return showChargingAnimation ? ((FullCircle - End) / ChargingSteps) * CurrentStep : FullCircle - End;
        }
    }
}