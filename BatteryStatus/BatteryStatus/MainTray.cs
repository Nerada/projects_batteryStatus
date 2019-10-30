//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191027
//-----------------------------------------------

using System;
using System.Globalization;
using System.Windows.Forms;
using BatteryStatus.IconHandling;

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
            icon.Text = $@"{batteryPercentage.ToString(CultureInfo.InvariantCulture)}%";

            SetUpdateInterval(isCharging: status.PowerLineStatus == PowerLineStatus.Online);
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