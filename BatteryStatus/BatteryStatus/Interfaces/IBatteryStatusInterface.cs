//-----------------------------------------------
//      Author: Ramon Bollen
//       File: BatteryStatus.Interfaces.IBatteryStatusInterface.cs
// Created on: 2019111
//-----------------------------------------------

using System;

namespace BatteryStatus.Interfaces
{
    internal interface IBatteryStatusInterface<T>
    {
        public event EventHandler<T> OnUpdate;

        public float Percentage { set; }
        public bool IsCharging { set; }
    }
}