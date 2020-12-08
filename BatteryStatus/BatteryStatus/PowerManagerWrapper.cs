// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.PowerManagerWrapper.cs
// Created on: 20191101
// -----------------------------------------------

using System;
using System.Timers;
using BatteryStatus.Interfaces;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace BatteryStatus
{
    internal class PowerManagerWrapper : IPowerManagerInterface
    {
        private readonly Timer _timeRemainingCheckTimer = new Timer();

        public PowerManagerWrapper()
        {
            PowerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            PowerManager.PowerSourceChanged        += PowerManager_PowerSourceChanged;

            _timeRemainingCheckTimer.Elapsed  += TimeRemainingCheckTimer_Elapsed;
            _timeRemainingCheckTimer.Interval =  new TimeSpan(0, 0, 5).TotalMilliseconds;
            _timeRemainingCheckTimer.Start();
        }

        public bool IsAvailable => PowerManager.IsBatteryPresent;

        public EventHandler? BatteryLifePercentChanged { get; set; }
        public EventHandler? PowerSourceChanged        { get; set; }
        public EventHandler? TimeRemainingChanged      { get; set; }

        public float BatteryLifePercent
        {
            get
            {
                float percentage = IsAvailable ? PowerManager.BatteryLifePercent : 100;

                if (percentage < 0) return 0;

                if (percentage > 100) return 100;

                return percentage;
            }
        }

        public bool IsCharging => PowerManager.PowerSource == PowerSource.AC;

        public TimeSpan TimeRemaining { get; private set; }

        private void PowerManager_BatteryLifePercentChanged(object? sender, EventArgs e)
        {
            if (!(BatteryLifePercentChanged is {} batteryLifePercentChanged)) return;

            batteryLifePercentChanged(sender, e);
        }

        private void PowerManager_PowerSourceChanged(object? sender, EventArgs e)
        {
            if (!(PowerSourceChanged is {} powerSourceChanged)) return;

            powerSourceChanged(sender, e);
        }

        private void TimeRemainingCheckTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (IsCharging) return;

            TimeSpan newRemaining = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
            if (TimeRemaining != newRemaining)
            {
                TimeRemaining = newRemaining;

                if (!(TimeRemainingChanged is { } timeRemainingChanged)) return;

                timeRemainingChanged(this, EventArgs.Empty);
            }
        }
    }
}