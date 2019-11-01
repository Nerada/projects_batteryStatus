using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Documents;
using BatteryStatus.Support;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace BatteryStatus.TextHandling
{
    internal class TextHandler
    {
        public event EventHandler<TextCreatedEventArgs> TextUpdated;

        public bool IsCharging;

        public void Update(float percentage)
        {
            TextUpdated(nameof(TextHandler), new TextCreatedEventArgs(AvailableText(percentage)));
        }

        public void Update(float percentage, TimeSpan remainingTime)
        {
            if (!IsCharging) { TextUpdated(nameof(TextHandler), new TextCreatedEventArgs(RemainingText(percentage, remainingTime))); }
            else
            {
                throw new Exception("Cannot update remaining time when charging");
            }
        }

        private static string AvailableText(float percentage)
        {
            return $"{percentage}% available";
        }

        private static string RemainingText(float percentage, TimeSpan remainingTime)
        {
            bool isRemainingTimeKnown = remainingTime > new TimeSpan();

            string hours              = remainingTime.Hours != 0 ? $"{remainingTime.Hours}hr" : string.Empty;
            string multiPart          = remainingTime.Hours > 1 ? "s" : string.Empty;
            string minutes            = remainingTime.Minutes != 0 ? $"{remainingTime.Minutes:00}min" : string.Empty;

            string timeText           = isRemainingTimeKnown ? $"{hours}{multiPart} {minutes}" : string.Empty;
            string percentageText     = isRemainingTimeKnown ? $" ({percentage}%)" : $"{percentage}%";

            return $"{timeText}{percentageText} remaining";
        }
    }
}