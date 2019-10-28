//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryPercentage.IconHandler.cs
// Created on: 20191027
//-----------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BatteryStatus
{
    /// <summary>
    /// Create icons
    /// </summary>
    internal class IconHandler : IDisposable
    {
        private readonly Bitmap _iconBitmap = new Bitmap(width:256, height:256);

        public Icon Create(float percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException();
            }

            ClearHandles();
            Graphics graphic = Graphics.FromImage(_iconBitmap);
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(
                pen: new Pen(Color.White, width: 48),
                rect: new Rectangle(x: 32, y: 40, width: 192, height: 192),
                startAngle: 270.0F,
                sweepAngle: (360.0F / 100.0F) * percentage);

            return Icon.FromHandle(_iconBitmap.GetHicon());
        }

        public void Dispose()
        {
            ClearHandles();
            _iconBitmap.Dispose();
        }

        private void ClearHandles()
        {
            DestroyIcon(handle: _iconBitmap.GetHicon());
            DestroyIcon(handle: _iconBitmap.GetHbitmap());
        }

        [System.Runtime.InteropServices.DllImport(dllName: "user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);
    }
}