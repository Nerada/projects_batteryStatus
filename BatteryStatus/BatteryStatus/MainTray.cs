//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191027
//-----------------------------------------------

using BatteryPercentage;

using Hardcodet.Wpf.TaskbarNotification;

using System;
using System.Windows.Forms;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly Timer _getBatteryInformationTimer = new Timer();
        private readonly TaskbarIcon _taskBarIcon = new TaskbarIcon();

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus.BatteryLifePercent * 100);

            _getBatteryInformationTimer.Tick += BatteryInformationTimer_Tick;
            _getBatteryInformationTimer.Interval = (int)new TimeSpan(0, 0, 0, 0, 200).TotalMilliseconds;
            _getBatteryInformationTimer.Start();
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }

        private static void UpdateStatus(TaskbarIcon icon, float perc)
        {
            icon.Icon = IconHandler.Create(perc);
            icon.ToolTipText = $"{perc.ToString()}%";
        }

        public void Dispose()
        {
            _getBatteryInformationTimer.Dispose();
            _taskBarIcon.Dispose();
        }
    }
}