// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.MainTray.cs
// Created on: 20200215
// -----------------------------------------------

using System;
using System.Windows.Forms;
using BatteryStatus.IconHandling;
using BatteryStatus.Support;
using BatteryStatus.TextHandling;

namespace BatteryStatus
{
    internal class MainTray : IDisposable
    {
        private readonly IconHandler         _iconHandler  = new IconHandler();
        private readonly PowerManagerWrapper _powerManager = new PowerManagerWrapper();

        private readonly NotifyIcon  _taskBarIcon = new NotifyIcon();
        private readonly TextHandler _textHandler = new TextHandler();

        /// <summary>
        ///     Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            _taskBarIcon.Click   += TaskBarIcon_Click;
            _taskBarIcon.Visible =  true;

            _powerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            _powerManager.PowerSourceChanged        += PowerManager_PowerSourceChanged;
            _powerManager.TimeRemainingChanged      += PowerManager_TimeRemainingChanged;

            _iconHandler.OnUpdate += IconHandler_OnUpdate;
            _textHandler.OnUpdate += TextHandler_OnUpdate;

            Initialize();
        }

        public void Dispose()
        {
            _taskBarIcon.Dispose();
            _iconHandler.Dispose();
        }

        private void Initialize()
        {
            PowerManager_BatteryLifePercentChanged(this, EventArgs.Empty);
            PowerManager_PowerSourceChanged(this, EventArgs.Empty);
        }

        private void PowerManager_BatteryLifePercentChanged(object sender, EventArgs e)
        {
            _iconHandler.Percentage = PowerManagerWrapper.BatteryLifePercent;
            _textHandler.Percentage = PowerManagerWrapper.BatteryLifePercent;
        }

        private void PowerManager_PowerSourceChanged(object sender, EventArgs e)
        {
            _iconHandler.IsCharging = PowerManagerWrapper.IsCharging;
            _textHandler.IsCharging = PowerManagerWrapper.IsCharging;
        }

        private void PowerManager_TimeRemainingChanged(object sender, EventArgs e)
        {
            _textHandler.RemainingTime = _powerManager.TimeRemaining;
        }

        private void IconHandler_OnUpdate(object sender, IconEventArgs e) { _taskBarIcon.Icon = e.Icon; }

        private void TextHandler_OnUpdate(object sender, TextEventArgs e) { _taskBarIcon.Text = e.Text; }

        private void TaskBarIcon_Click(object sender, EventArgs e)
        {
            _iconHandler.ShowChargingAnimation = !_iconHandler.ShowChargingAnimation;
        }
    }
}