// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.TextHandler.cs
// Created on: 20200215
// -----------------------------------------------

using System;
using BatteryStatus.Exceptions;
using BatteryStatus.Support;

namespace BatteryStatus.TextHandling
{
    /// <summary>
    ///     Create text for hover on tray icon.
    /// </summary>
    internal class TextHandler
    {
        private bool     _isCharging;
        private float    _percentage;
        private TimeSpan _remainingTime;

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
                if (value < 0 || value > 100) throw new PropertyOutOfRangeException();

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

        public event EventHandler<TextEventArgs> OnUpdate;

        private void Update()
        {
            OnUpdate?.Invoke(this, !IsCharging ? new TextEventArgs(RemainingText()) : new TextEventArgs(AvailableText()));
        }

        private string AvailableText() => $"{Percentage}% available";

        private string RemainingText()
        {
            bool isRemainingTimeKnown = RemainingTime > new TimeSpan();

            string hours     = RemainingTime.Hours   != 0 ? $"{RemainingTime.Hours}hr" : string.Empty;
            string multiPart = RemainingTime.Hours   > 1 ? "s" : string.Empty;
            string minutes   = RemainingTime.Minutes != 0 ? $"{RemainingTime.Minutes:00}min" : string.Empty;

            string timeText       = isRemainingTimeKnown ? $"{hours}{multiPart} {minutes}" : string.Empty;
            string percentageText = isRemainingTimeKnown ? $" ({Percentage}%)" : $"{Percentage}%";

            return $"{timeText}{percentageText} remaining";
        }
    }
}