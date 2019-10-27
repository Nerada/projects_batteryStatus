//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191027
//-----------------------------------------------

using BatteryPercentage;

using System;
using System.Windows.Forms;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly Timer _getBatteryInformationTimer = new Timer();
        private readonly NotifyIcon _taskBarIcon = new NotifyIcon();

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus.BatteryLifePercent * 100);
            _taskBarIcon.Visible = true;

            _getBatteryInformationTimer.Tick += BatteryInformationTimer_Tick;
            _getBatteryInformationTimer.Interval = (int)new TimeSpan(hours: 0, minutes: 0, seconds: 5).TotalMilliseconds;
            _getBatteryInformationTimer.Start();
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }

        private static void UpdateStatus(NotifyIcon icon, float perc)
        {
            icon.Icon = IconHandler.Create(perc);
            icon.Text = $"{perc.ToString()}%";
        }

        public void Dispose()
        {
            _getBatteryInformationTimer.Dispose();
            _taskBarIcon.Dispose();
        }
    }
}