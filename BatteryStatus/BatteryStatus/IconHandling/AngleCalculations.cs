// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.AngleCalculations.cs
// Created on: 20191030
// -----------------------------------------------

using System.Runtime.CompilerServices;
using BatteryStatus.Exceptions;

[assembly: InternalsVisibleTo("BatteryStatus.tests")]

namespace BatteryStatus.IconHandling
{
    /// <summary>
    ///     Angle calculations for drawing.
    /// </summary>
    internal class AngleCalculations
    {
        private const float FullCircle = 360.0F;
        private const float StartPoint = 270.0F;

        private const int   ChargingSteps = 3;
        private       int   _currentStep;
        private       float _percentage;

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
                if (!(value >= 0 && value <= 100)) throw new PropertyOutOfRangeException();

                _percentage = value;
            }
        }

        public float Start => StartPoint;

        public float End => FullCircle / 100 * Percentage;

        public float Start2 => Start + End;

        public float End2(bool showChargingAnimation)
        {
            if (showChargingAnimation)
            {
                CurrentStep++;
            }
            else
            {
                CurrentStep = 0;
            }

            return showChargingAnimation ? (FullCircle - End) / ChargingSteps * CurrentStep : FullCircle - End;
        }
    }
}