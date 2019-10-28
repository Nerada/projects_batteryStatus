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

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(_taskBarIcon, percentage: SystemInformation.PowerStatus.BatteryLifePercent * 100);
            _taskBarIcon.Visible = true;

            _getBatteryInformationTimer.Tick += BatteryInformationTimer_Tick;
            _getBatteryInformationTimer.Interval = (int)new TimeSpan(hours: 0, minutes: 0, seconds: 5).TotalMilliseconds;
            _getBatteryInformationTimer.Start();
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, percentage: SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }

        private void UpdateStatus(NotifyIcon icon, float percentage)
        {
            icon.Icon = _iconHandler.Create(percentage);
            icon.Text = $@"{percentage.ToString(CultureInfo.InvariantCulture)}%";
        }

        public void Dispose()
        {
            _getBatteryInformationTimer.Dispose();
            _taskBarIcon.Dispose();
        }
    }
}