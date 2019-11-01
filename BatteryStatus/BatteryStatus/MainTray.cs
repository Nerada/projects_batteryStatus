//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20191030
//-----------------------------------------------

using BatteryStatus.IconHandling;

using System;
using System.Globalization;
using System.Windows.Forms;
using BatteryStatus.TextHandling;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly Timer       _getBatteryInfoTimer = new Timer();
        private readonly NotifyIcon  _taskBarIcon         = new NotifyIcon();
        private readonly IconHandler _iconHandler         = new IconHandler();

        private readonly TextHandler _textHandler = new TextHandler();

        /// <summary>
        /// Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            UpdateStatus(icon: _taskBarIcon, status: SystemInformation.PowerStatus);
            _taskBarIcon.Click += TaskBarIcon_Click;
            _taskBarIcon.Visible = true;

            PowerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            PowerManager.PowerSourceChanged += PowerManager_PowerSourceChanged;

            _iconHandler.IconUpdated += IconHandler_IconUpdated;
            _textHandler.TextUpdated += _textHandler_TextUpdated;
        }

        private void PowerManager_BatteryLifePercentChanged(object sender, EventArgs e)
        {
            _iconHandler.Percentage = PowerManager.BatteryLifePercent;
            _textHandler.Update(percentage: PowerManager.BatteryLifePercent, PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining);
        }

        private void PowerManager_PowerSourceChanged(object sender, EventArgs e)
        {
            _iconHandler.IsCharging = PowerManager.PowerSource == PowerSource.AC;
            _textHandler.IsCharging = PowerManager.PowerSource == PowerSource.AC;
        }

        private void IconHandler_IconUpdated(object sender, Support.IconCreatedEventArgs e)
        {
            _taskBarIcon.Icon = e.Icon;
        }

        private void _textHandler_TextUpdated(object sender, Support.TextCreatedEventArgs e)
        {
            throw new NotImplementedException();
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

            icon.Text = CreateIconText(status);

            SetUpdateInterval(isCharging: status.PowerLineStatus == PowerLineStatus.Online);
        }

        private void TaskBarIcon_Click(object sender, EventArgs e)
        {
            _iconHandler.ShowChargingAnimation = !_iconHandler.ShowChargingAnimation;
        }
    }
}