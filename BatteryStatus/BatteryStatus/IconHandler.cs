//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryPercentage.IconHandler.cs
// Created on: 20191027
//-----------------------------------------------

using System;
using System.Drawing;

namespace BatteryPercentage
{
    /// <summary>
    /// Create icons
    /// </summary>
    internal static class IconHandler
    {
        public static Icon Create(float percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException();
            }

            using var iconBitmap = new Bitmap(256, 256);

            var graphic = Graphics.FromImage(iconBitmap);
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(
                pen: new Pen(Color.White, 48),
                rect: new Rectangle(32, 40, 192, 192),
                startAngle: 270.0F,
                sweepAngle: (360.0F / 100.0F) * percentage);

            return Icon.FromHandle(iconBitmap.GetHicon());
        }
    }
}