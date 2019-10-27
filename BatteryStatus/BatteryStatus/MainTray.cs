//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191027
//-----------------------------------------------

using Hardcodet.Wpf.TaskbarNotification;

using System;
using System.Drawing;
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
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus);

            _getBatteryInformationTimer.Tick += BatteryInformationTimer_Tick;
            _getBatteryInformationTimer.Interval = (int)new TimeSpan(0, 0, 1).TotalMilliseconds;
            _getBatteryInformationTimer.Start();
        }

        private void BatteryInformationTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(_taskBarIcon, SystemInformation.PowerStatus);
        }

        private static void UpdateStatus(TaskbarIcon icon, PowerStatus status)
        {
            float battPerc = status.BatteryLifePercent * 100;

            icon.Icon = CreateIcon(battPerc);
            icon.ToolTipText = $"{battPerc.ToString()}%";
        }

        private static Icon CreateIcon(float percentage)
        {
            var bitmap = new Bitmap(256, 256);
            var graphic = Graphics.FromImage(bitmap);
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(
                new Pen(Color.White, 48),
                new Rectangle(32, 40, 192, 192),
                270.0F,
                (360.0F / 100.0F) * percentage);

            return Icon.FromHandle(bitmap.GetHicon());
        }

        public void Dispose()
        {
            _getBatteryInformationTimer.Dispose();
            _taskBarIcon.Dispose();
        }
    }
}