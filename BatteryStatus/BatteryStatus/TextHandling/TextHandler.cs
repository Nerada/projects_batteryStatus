//-----------------------------------------------
//      Author: Ramon Bollen
//       File: BatteryStatus.TextHandling.TextHandler.cs
// Created on: 2019111
//-----------------------------------------------

using BatteryStatus.Exceptions;
using BatteryStatus.Support;

using System;

namespace BatteryStatus.TextHandling
{
    /// <summary>
    /// Create text for hover on tray icon.
    /// </summary>
    internal class TextHandler
    {
        private float    _percentage;
        private bool     _isCharging;
        private TimeSpan _remainingTime;

        public event EventHandler<TextEventArgs> OnUpdate;

        public bool IsCharging
        {
            private get => _isCharging;
            set
            {
                _isCharging = value;
                Update();
            }
        }

        public float Percentage
        {
            private get => _percentage;
            set
            {
                if (value < 0 || value > 100) { throw new PropertyOutOfRangeException(); }
                _percentage = value;
                Update();
            }
        }

        public TimeSpan RemainingTime
        {
            private get => _remainingTime;
            set
            {
                _remainingTime = value;
                Update();
            }
        }

        private void Update()
        {
            if (!IsCharging) { OnUpdate(this, new TextEventArgs(RemainingText())); }
            else { OnUpdate(this, new TextEventArgs(AvailableText())); }
        }

        private string AvailableText()
        {
            return $"{Percentage}% available";
        }

        private string RemainingText()
        {
            bool isRemainingTimeKnown = RemainingTime > new TimeSpan();

            string hours              = RemainingTime.Hours != 0   ? $"{RemainingTime.Hours}hr"      : string.Empty;
            string multiPart          = RemainingTime.Hours > 1    ? "s"                             : string.Empty;
            string minutes            = RemainingTime.Minutes != 0 ? $"{RemainingTime.Minutes:00}min": string.Empty;

            string timeText           = isRemainingTimeKnown       ? $"{hours}{multiPart} {minutes}" : string.Empty;
            string percentageText     = isRemainingTimeKnown       ? $" ({Percentage}%)"             : $"{Percentage}%";

            return $"{timeText}{percentageText} remaining";
        }
    }
}