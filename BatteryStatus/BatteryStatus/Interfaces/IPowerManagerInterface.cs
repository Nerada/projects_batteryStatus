// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IPowerManagerInterface.cs
// Created on: 20201207
// -----------------------------------------------

using System;

namespace BatteryStatus.Interfaces
{
    internal interface IPowerManagerInterface
    {
        bool IsAvailable { get; }

        EventHandler? BatteryLifePercentChanged { get; set; }

        EventHandler? PowerSourceChanged { get; set; }

        EventHandler? TimeRemainingChanged { get; set; }

        float BatteryLifePercent { get; }

        bool IsCharging { get; }

        TimeSpan TimeRemaining { get; }
    }
}