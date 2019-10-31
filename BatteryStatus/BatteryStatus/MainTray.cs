//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191030
//-----------------------------------------------

using BatteryStatus.IconHandling;

using System;
using System.Globalization;
using System.Windows.Forms;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly Timer       _getBatteryInfoTimer = new Timer();
        private readonly NotifyIcon  _taskBarIcon         = new NotifyIcon();
        private readonly IconHandler _iconHandler         = new IconHandler();

        private static TimeSpan      _fastUpdate          = new TimeSpan(hours: 0, minutes: 0, seconds: 1);
        private static TimeSpan      _slowUpdate          = new TimeSpan(hours: 0, minutes: 0, seconds: 5);

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(icon: _taskBarIcon, status: SystemInformation.PowerStatus);
            _taskBarIcon.Click += TaskBarIcon_Click;
            _taskBarIcon.Visible = true;

            _getBatteryInfoTimer.Tick += BatteryInformationTimer_Tick;
            SetUpdateInterval(SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online);
            _getBatteryInfoTimer.Start();
        }

        public void Dispose()
        {
            _getBatteryInfoTimer.Dispose();
            _taskBarIcon.Dispose();
            _iconHandler.Dispose();
        }

        private void UpdateStatus(NotifyIcon icon, PowerStatus status)
        {
            float batteryPercentage = status.BatteryLifePercent * 100;
            icon.Icon = _iconHandler.Update(percentage: batteryPercentage, isCharging: status.PowerLineStatus == PowerLineStatus.Online);

            icon.Text = CreateIconText(status);

            SetUpdateInterval(isCharging: status.PowerLineStatus == PowerLineStatus.Online);
        }

        private string CreateIconText(PowerStatus status)
        {
            string percentage         = (status.BatteryLifePercent * 100).ToString(CultureInfo.InvariantCulture);
            var remainingTime         = new TimeSpan(0, 0, status.BatteryLifeRemaining);
            bool isRemainingTimeKnown = remainingTime > new TimeSpan();

            string hours              = remainingTime.Hours != 0 ? $"{remainingTime.Hours}hr"                     : string.Empty;
            string multiPart          = remainingTime.Hours > 1 ? "s"                                             : string.Empty;
            string minutes            = remainingTime.Minutes != 0 ? $"{remainingTime.Minutes:00}min" : string.Empty;

            string timeText           = isRemainingTimeKnown ? $"{hours}{multiPart} {minutes}"                    : string.Empty;
            string percentageText     = isRemainingTimeKnown ? $" ({percentage}%)"                                : $"{percentage}%";
            string chargingText       = status.PowerLineStatus == PowerLineStatus.Online ? "available"            : "remaining";

            return $"{timeText}{percentageText} {chargingText}";
        }

        private void SetUpdateInterval(bool isCharging)
        {
            _getBatteryInfoTimer.Interval = isCharging && _iconHandler.ShowChargingAnimation
                ? (int)_fastUpdate.TotalMilliseconds : (int)_slowUpdate.TotalMilliseconds;
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, status: SystemInformation.PowerStatus);
        }

        private void TaskBarIcon_Click(object sender, EventArgs e)
        {
            _iconHandler.ShowChargingAnimation = !_iconHandler.ShowChargingAnimation;
        }
    }
}