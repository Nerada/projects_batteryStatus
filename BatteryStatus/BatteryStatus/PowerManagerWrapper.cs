//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.BatteryManager.cs
// Created on: 2019111
//-----------------------------------------------

using Microsoft.WindowsAPICodePack.ApplicationServices;

using System;
using System.Timers;

namespace BatteryStatus
{
    internal class PowerManagerWrapper
    {
        private readonly Timer _timeRemainingCheckTimer = new Timer();
        private TimeSpan _timeRemaining = new TimeSpan();

        public PowerManagerWrapper()
        {
            PowerManager.BatteryLifePercentChanged += PowerManager_BatteryLifePercentChanged;
            PowerManager.PowerSourceChanged += PowerManager_PowerSourceChanged;

            _timeRemainingCheckTimer.Elapsed += TimeRemainingCheckTimer_Elapsed;
            _timeRemainingCheckTimer.Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 5).TotalMilliseconds;
            _timeRemainingCheckTimer.Start();
        }

        public EventHandler BatteryLifePercentChanged;
        public EventHandler PowerSourceChanged;
        public EventHandler TimeRemainingChanged;

        public float BatteryLifePercent
        {
            get
            {
                float perc = PowerManager.BatteryLifePercent;
                if (perc < 0) { return 0; }
                if (perc > 100) { return 100; }
                return perc;
            }
        }

        public bool IsCharging => PowerManager.PowerSource == PowerSource.AC;

        public TimeSpan TimeRemaining => _timeRemaining;

        private void PowerManager_BatteryLifePercentChanged(object sender, EventArgs e)
        {
            BatteryLifePercentChanged(sender, e);
        }

        private void PowerManager_PowerSourceChanged(object sender, EventArgs e)
        {
            PowerSourceChanged(sender, e);
        }

        private void TimeRemainingCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsCharging) { return; }

            TimeSpan newRemaining = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
            if (_timeRemaining != newRemaining)
            {
                _timeRemaining = newRemaining;
                TimeRemainingChanged(this, EventArgs.Empty);
            }
        }
    }
}