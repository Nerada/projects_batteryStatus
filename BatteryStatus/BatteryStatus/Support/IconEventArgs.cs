//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.Support.IconEventArgs.cs
// Created on: 2019111
//-----------------------------------------------

using System.Drawing;

namespace BatteryStatus.Support
{
    internal class IconEventArgs
    {
        public IconEventArgs(Icon icon) => Icon = icon;

        public Icon Icon { get; }
    }
}