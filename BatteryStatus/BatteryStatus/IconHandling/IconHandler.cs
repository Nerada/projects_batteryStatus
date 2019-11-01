//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.IconHandling.IconHandler.cs
// Created on: 2019111
//-----------------------------------------------

using BatteryStatus.Exceptions;
using BatteryStatus.Interfaces;
using BatteryStatus.Support;

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;

namespace BatteryStatus.IconHandling
{
    /// <summary>
    /// Create icons.
    /// </summary>
    internal class IconHandler : IBatteryStatusInterface<IconEventArgs>, IDisposable
    {
        private readonly Timer             _chargeTimer  = new Timer();

        private float                      _percentage;
        private bool                       _isCharging;
        private bool                       _showAnimation;

        private readonly Bitmap            _iconBitmap   = new Bitmap(width: 256, height: 256);
        private readonly AngleCalculations _calculations = new AngleCalculations();
        private readonly Rectangle         _boundaries   = new Rectangle(x: 32, y: 40, width: 192, height: 192);
        private Icon                       _generatedIcon;
        private const int                  PenWidth      = 48;

        public IconHandler()
        {
            _chargeTimer.Elapsed += ChargeTimer_Elapsed;
            _chargeTimer.Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 1).TotalMilliseconds;
        }

        public event EventHandler<IconEventArgs> OnUpdate;

        public float Percentage
        {
            private get => _percentage;
            set
            {
                if (value < 0 || value > 100) { throw new PropertyOutOfRangeException(); }

                _percentage = value;
                Update();
            }
        }

        public bool IsCharging
        {
            private get => _isCharging;
            set
            {
                _isCharging = value;
                if (!_isCharging) { ShowChargingAnimation = false; }
                if (!ShowChargingAnimation) { Update(); }
            }
        }

        public bool ShowChargingAnimation
        {
            get => _showAnimation;
            set
            {
                if (IsCharging) { _showAnimation = value; }
                else { _showAnimation = false; }

                if (_showAnimation) { _chargeTimer.Start(); }
                else
                {
                    _chargeTimer.Stop();
                    Update();
                }
            }
        }

        public void Dispose()
        {
            _chargeTimer.Close();
            _iconBitmap.Dispose();
            DestroyIcon(_generatedIcon);
            _generatedIcon.Dispose();
        }

        private void Update()
        {
            DestroyIcon(_generatedIcon);
            _generatedIcon = Icon.FromHandle(Draw());

            OnUpdate(this, new IconEventArgs(_generatedIcon));
        }

        private IntPtr Draw()
        {
            var graphic = Graphics.FromImage(_iconBitmap);

            _calculations.Percentage = Percentage;

            DrawBattery(graphic);
            if (IsCharging) { DrawChargingStep(graphic); }

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
            if (icon != null) { DestroyIcon(icon.Handle); }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private void ChargeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }
    }
}