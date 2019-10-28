//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191027
//-----------------------------------------------

using System;
using System.Globalization;
using System.Windows.Forms;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly Timer _getBatteryInformationTimer = new Timer();
        private readonly NotifyIcon _taskBarIcon = new NotifyIcon();
        private readonly IconHandler _iconHandler = new IconHandler();

        private static TimeSpan _fastUpdate = new TimeSpan(hours: 0, minutes: 0, seconds: 1);
        private static TimeSpan _slowUpdate = new TimeSpan(hours: 0, minutes: 0, seconds: 5);

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(_taskBarIcon, status: SystemInformation.PowerStatus);
            _taskBarIcon.Click += _taskBarIcon_Click;
            _taskBarIcon.Visible = true;

            _getBatteryInformationTimer.Tick += BatteryInformationTimer_Tick;
            SetUpdateInverval(SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online);
            _getBatteryInformationTimer.Start();
        }

        public void Dispose()
        {
            _getBatteryInformationTimer.Dispose();
            _taskBarIcon.Dispose();
        }

        private void UpdateStatus(NotifyIcon icon, PowerStatus status)
        {
            float batteryPercentage = status.BatteryLifePercent * 100;
            icon.Icon = _iconHandler.Update(percentage: batteryPercentage, isCharging: status.PowerLineStatus == PowerLineStatus.Online);
            icon.Text = $"{batteryPercentage.ToString(CultureInfo.InvariantCulture)}%";

            SetUpdateInverval(isCharging: status.PowerLineStatus == PowerLineStatus.Online);
        }

        private void SetUpdateInverval(bool isCharging)
        {
            _getBatteryInformationTimer.Interval = isCharging && _iconHandler.ShowChargingAnimation
                ? (int)_fastUpdate.TotalMilliseconds : (int)_slowUpdate.TotalMilliseconds;
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, status: SystemInformation.PowerStatus);
        }

        private void _taskBarIcon_Click(object sender, EventArgs e)
        {
            _iconHandler.ShowChargingAnimation = !_iconHandler.ShowChargingAnimation;
        }
    }
}