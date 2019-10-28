//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryPercentage.IconHandler.cs
// Created on: 20191027
//-----------------------------------------------

using System;
using System.Drawing;

namespace BatteryStatus
{
    /// <summary>
    /// Create icons
    /// </summary>
    internal class IconHandler : IDisposable
    {
        private static readonly Bitmap _iconBitmap = new Bitmap(width: 256, height: 256);

        private static int _currentStep;
        private const  int ChargingSteps = 3;

        public bool ShowChargingAnimation { get; set; }

        private static int CurrentStep
        {
            get => _currentStep;
            set => _currentStep = value > ChargingSteps ? 0 : value;
        }

        public Icon Update(float percentage, bool isCharging)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException();
            }

            return Icon.FromHandle(Draw(percentage: percentage, isCharging: isCharging));
        }

        public void Dispose()
        {
            _iconBitmap.Dispose();
        }

        private IntPtr Draw(float percentage, bool isCharging)
        {
            float endPoint = (360.0F / 100.0F) * percentage;

            var graphic = Graphics.FromImage(_iconBitmap);
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(
                pen: new Pen(Color.White, width: 48),
                rect: new Rectangle(x: 32, y: 40, width: 192, height: 192),
                startAngle: 270.0F,
                sweepAngle: endPoint);

            if (isCharging) { DrawChargingStep(graphic, endPoint); } else { CurrentStep = 0; }

            return _iconBitmap.GetHicon();
        }

        private void DrawChargingStep(Graphics graphic, float mainEndPoint)
        {
            CurrentStep++;

            float startPoint = 270.0F + mainEndPoint;
            float endPoint = 360.0F - mainEndPoint;

            float stepSize = ShowChargingAnimation ? (endPoint / ChargingSteps) * CurrentStep : endPoint;

            graphic.DrawArc(
                pen: new Pen(Color.FromArgb(125, 255, 255, 255), width: 48),
                rect: new Rectangle(x: 32, y: 40, width: 192, height: 192),
                startAngle: startPoint,
                sweepAngle: stepSize);
        }
    }
}