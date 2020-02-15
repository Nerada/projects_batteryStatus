// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.IconEventArgs.cs
// Created on: 20191101
// -----------------------------------------------

using System.Drawing;

namespace BatteryStatus.Support
{
    internal class IconEventArgs
    {
        public IconEventArgs(Icon icon) => Icon = icon;

        public Icon Icon { get; }
    }
}