// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.PowerManagerWrapper.cs
// Created on: 20201207
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Timers;
using BatteryStatus.Interfaces;
using BatteryStatus.Support;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace BatteryStatus
{
    internal class PowerManagerWrapper : IPowerManagerInterface, IDisposable
    {
        private readonly Timer _timeRemainingCheckTimer = new();

        private readonly List<Action> _disposeActions = new();
        private          bool         _disposed;

        public PowerManagerWrapper()
        {
            EventHandler batteryLivePercentChangedEventHandler = (_, _) => PowerManager_BatteryLifePercentChanged();
            PowerManager.BatteryLifePercentChanged += batteryLivePercentChangedEventHandler;
            _disposeActions.Add(() => PowerManager.BatteryLifePercentChanged -= batteryLivePercentChangedEventHandler);

            EventHandler powerSourceChangedEventHandler = (_, _) => PowerManager_PowerSourceChanged();
            PowerManager.PowerSourceChanged += powerSourceChangedEventHandler;
            _disposeActions.Add(() => PowerManager.PowerSourceChanged -= powerSourceChangedEventHandler);

            ElapsedEventHandler timeRemainingEventHandler = (_, _) => TimeRemainingCheckTimer_Elapsed();
            _timeRemainingCheckTimer.Elapsed += timeRemainingEventHandler;

            _disposeActions.Add(() => _timeRemainingCheckTimer.Close());
            _disposeActions.Add(() => _timeRemainingCheckTimer.Elapsed -= timeRemainingEventHandler);
            _disposeActions.Add(() => _timeRemainingCheckTimer.Stop());

            _timeRemainingCheckTimer.Interval = new TimeSpan(0, 0, 5).TotalMilliseconds;
            _timeRemainingCheckTimer.Start();
        }

        public bool IsAvailable => PowerManager.IsBatteryPresent;

        public VoidEventHandler? BatteryLifePercentChanged { get; set; }
        public VoidEventHandler? PowerSourceChanged        { get; set; }
        public VoidEventHandler? TimeRemainingChanged      { get; set; }

        public float BatteryLifePercent
        {
            get
            {
                float percentage = IsAvailable ? PowerManager.BatteryLifePercent : 100;

                return percentage switch
                {
                    < 0   => 0,
                    > 100 => 100,
                    _     => percentage
                };
            }
        }

        public bool IsCharging => PowerManager.PowerSource == PowerSource.AC;

        public TimeSpan TimeRemaining { get; private set; }

        private void PowerManager_BatteryLifePercentChanged()
        {
            if (!(BatteryLifePercentChanged is { } batteryLifePercentChanged)) return;

            batteryLifePercentChanged();
        }

        private void PowerManager_PowerSourceChanged()
        {
            if (!(PowerSourceChanged is { } powerSourceChanged)) return;

            powerSourceChanged();
        }

        private void TimeRemainingCheckTimer_Elapsed()
        {
            if (IsCharging) return;

            TimeSpan newRemaining = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
            if (TimeRemaining != newRemaining)
            {
                TimeRemaining = newRemaining;

                if (!(TimeRemainingChanged is { } timeRemainingChanged)) return;

                timeRemainingChanged();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            _disposeActions.Reverse();
            _disposeActions.ForEach(a => a());
            _disposeActions.Clear();
        }
    }
}