// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.IBatteryStatusInterface.cs
// Created on: 20191101
// -----------------------------------------------

using System;

namespace BatteryStatus.Interfaces
{
    internal interface IBatteryStatusInterface<T>
    {
        float Percentage { set; }

        bool IsCharging { set; }

        event EventHandler<T> OnUpdate;
    }
}