﻿// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.PowerManagerWrapper.cs
// Created on: 20191101
// -----------------------------------------------

using System;
using System.Timers;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace BatteryStatus
{
    internal class PowerManagerWrapper
    {
        private readonly Timer _timeRemainingCheckTimer = new Timer();

        public EventHandler BatteryLifePercentChanged;
        public EventHandler PowerSourceChanged;
        public EventHandler TimeRemainingChanged;

        public PowerManagerWrapper()
        {
            PowerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            PowerManager.PowerSourceChanged        += PowerManager_PowerSourceChanged;

            _timeRemainingCheckTimer.Elapsed  += TimeRemainingCheckTimer_Elapsed;
            _timeRemainingCheckTimer.Interval =  new TimeSpan(0, 0, 5).TotalMilliseconds;
            _timeRemainingCheckTimer.Start();
        }

        public static float BatteryLifePercent
        {
            get
            {
                float percentage = PowerManager.BatteryLifePercent;
                if (percentage < 0) { return 0; }

                if (percentage > 100) { return 100; }

                return percentage;
            }
        }

        public static bool IsCharging => PowerManager.PowerSource == PowerSource.AC;

        public TimeSpan TimeRemaining { get; private set; }

        private void PowerManager_BatteryLifePercentChanged(object sender, EventArgs e) { BatteryLifePercentChanged(sender, e); }

        private void PowerManager_PowerSourceChanged(object sender, EventArgs e) { PowerSourceChanged(sender, e); }

        private void TimeRemainingCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsCharging) { return; }

            var newRemaining = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
            if (TimeRemaining != newRemaining)
            {
                TimeRemaining = newRemaining;
                TimeRemainingChanged(this, EventArgs.Empty);
            }
        }
    }
}