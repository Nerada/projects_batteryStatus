//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.IconHandling.IconHandler.cs
// Created on: 20191030
//-----------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BatteryStatus.IconHandling
{
    /// <summary>
    /// Create icons.
    /// </summary>
    internal class IconHandler : IDisposable
    {
        private readonly Bitmap            _iconBitmap   = new Bitmap(width: 256, height: 256);
        private readonly AngleCalculations _calculations = new AngleCalculations();
        private readonly Rectangle         _boundaries   = new Rectangle(x: 32, y: 40, width: 192, height: 192);
        private Icon                       _generatedIcon;
        private const int                  PenWidth      = 48;

        public bool ShowChargingAnimation { get; set; }

        public Icon Update(float percentage, bool isCharging)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException();
            }

            DestroyIcon(_generatedIcon);
            _generatedIcon = Icon.FromHandle(Draw(percentage, isCharging));

            return _generatedIcon;
        }

        public void Dispose()
        {
            _iconBitmap.Dispose();
            DestroyIcon(_generatedIcon);
            _generatedIcon.Dispose();
        }

        private IntPtr Draw(float percentage, bool isCharging)
        {
            var graphic = Graphics.FromImage(_iconBitmap);

            _calculations.Percentage = percentage;

            DrawBattery(graphic);
            if (isCharging) { DrawChargingStep(graphic); }

            return _iconBitmap.GetHicon();
        }

        private void DrawBattery(Graphics graphic)
        {
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(
                pen: new Pen(Color.White, width: PenWidth),
                rect: _boundaries,
                startAngle: _calculations.Start,
                sweepAngle: _calculations.End);
        }

        private void DrawChargingStep(Graphics graphic)
        {
            graphic.DrawArc(
                pen: new Pen(Color.FromArgb(125, 255, 255, 255), width: PenWidth),
                rect: _boundaries,
                startAngle: _calculations.Start2,
                sweepAngle: _calculations.End2(ShowChargingAnimation));
        }

        private static void DestroyIcon(Icon icon)
        {
            if (icon != null)
            {
                DestroyIcon(icon.Handle);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);
    }
}