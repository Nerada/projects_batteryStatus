//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.IconHandling.AngleCalculations.cs
// Created on: 20191029
//-----------------------------------------------

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

        private int CurrentStep
        {
            get => _currentStep;
            set => _currentStep = value > ChargingSteps ? 0 : value;
        }

        public float Percentage { get; set; } = 0;

        public float Start => StartPoint;

        public float End => (FullCircle / 100) * Percentage;

        public float Start2 => StartPoint + End;

        public float End2(bool ShowChargingAnimation)
        {
            if (ShowChargingAnimation) { CurrentStep++; } else { CurrentStep = 0; }
            return ShowChargingAnimation ? ((FullCircle - End) / ChargingSteps) * CurrentStep : FullCircle - End;
        }
    }
}