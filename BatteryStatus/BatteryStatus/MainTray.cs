// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.MainTray.cs
// Created on: 20201207
// -----------------------------------------------

using System;
using System.Windows.Forms;
using BatteryStatus.IconHandling;
using BatteryStatus.Interfaces;
using BatteryStatus.Support;
using BatteryStatus.TextHandling;

namespace BatteryStatus
{
    public class MainTray : IDisposable
    {
        private readonly IIconHandlerInterface<IconEventArgs> _iconHandler = new IconHandler();

        private readonly IPowerManagerInterface _powerManager = new PowerManagerWrapper();

        private readonly AwakeModeHelper _awakeModeHelper = new();

        private readonly NotifyIcon  _taskBarIcon = new();
        private readonly TextHandler _textHandler = new();

        private bool _disposed;

        /// <summary>
        ///     Get system battery status and update the tray icon.
        /// </summary>
        public MainTray()
        {
            _taskBarIcon.MouseClick += TaskBarIcon_Click;
            _taskBarIcon.Visible    =  true;

            _powerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            _powerManager.PowerSourceChanged        += PowerManager_PowerSourceChanged;
            _powerManager.TimeRemainingChanged      += PowerManager_TimeRemainingChanged;

            _iconHandler.OnUpdate += IconHandler_OnUpdate;
            _textHandler.OnUpdate += TextHandler_OnUpdate;

            Initialize();
        }

        private void Initialize()
        {
            PowerManager_BatteryLifePercentChanged(this, EventArgs.Empty);
            PowerManager_PowerSourceChanged(this, EventArgs.Empty);
        }

        private void PowerManager_BatteryLifePercentChanged(object? sender, EventArgs e)
        {
            _iconHandler.Percentage = _powerManager.BatteryLifePercent;
            _textHandler.Percentage = _powerManager.BatteryLifePercent;
        }

        private void PowerManager_PowerSourceChanged(object? sender, EventArgs e)
        {
            _iconHandler.IsCharging = _powerManager.IsCharging;
            _textHandler.IsCharging = _powerManager.IsCharging;
        }

        private void PowerManager_TimeRemainingChanged(object? sender, EventArgs e)
        {
            _textHandler.RemainingTime = _powerManager.TimeRemaining;
        }

        private void IconHandler_OnUpdate(object? sender, IconEventArgs e) => _taskBarIcon.Icon = e.Icon;

        private void TextHandler_OnUpdate(object? sender, TextEventArgs e) => _taskBarIcon.Text = e.Text;

        private void TaskBarIcon_Click(object? sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    _iconHandler.StayAwake = !_iconHandler.StayAwake;

                    if (_iconHandler.StayAwake)
                    {
                        if (!_awakeModeHelper.ForceSystemAwake()) _iconHandler.StayAwake = false;
                    }
                    else if (!_awakeModeHelper.ResetSystemDefault()) _iconHandler.StayAwake = true;

                    break;
                }
                case MouseButtons.Right:
                {
                    _iconHandler.ShowChargingAnimation = !_iconHandler.ShowChargingAnimation;
                    break;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            _taskBarIcon.Dispose();
            _iconHandler.Dispose();
        }
    }
}