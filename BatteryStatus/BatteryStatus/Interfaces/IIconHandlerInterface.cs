// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IIconHandlerInterface.cs
// Created on: 20201207
// -----------------------------------------------

using System;

namespace BatteryStatus.Interfaces
{
    internal interface IIconHandlerInterface<T>
    {
        bool ShowChargingAnimation { get; set; }

        float Percentage { set; }

        bool IsCharging { set; }

        event EventHandler<T> OnUpdate;

        void Dispose();
    }
}