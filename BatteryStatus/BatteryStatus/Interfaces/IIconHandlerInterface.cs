﻿// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IIconHandlerInterface.cs
// Created on: 20201207
// -----------------------------------------------

using System;

namespace BatteryStatus.Interfaces
{
    internal interface IIconHandlerInterface<T>
    {
        bool ShowChargingAnimation { set; }

        float Percentage { set; }

        bool IsCharging { set; }

        bool StayAwake { get; set; }

        event EventHandler<T> OnUpdate;

        void Dispose();
    }
}