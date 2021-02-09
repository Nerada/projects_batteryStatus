// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IPowerManagerInterface.cs
// Created on: 20201207
// -----------------------------------------------

using System;
using BatteryStatus.Support;

namespace BatteryStatus.Interfaces
{
    internal interface IPowerManagerInterface
    {
        bool IsAvailable { get; }

        VoidEventHandler? BatteryLifePercentChanged { get; set; }

        VoidEventHandler? PowerSourceChanged { get; set; }

        VoidEventHandler? TimeRemainingChanged { get; set; }

        float BatteryLifePercent { get; }

        bool IsCharging { get; }

        TimeSpan TimeRemaining { get; }
    }
}