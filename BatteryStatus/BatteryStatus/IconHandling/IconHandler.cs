// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.IconHandler.cs
// Created on: 20200215
// -----------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using BatteryStatus.Exceptions;
using BatteryStatus.Interfaces;
using BatteryStatus.Support;

namespace BatteryStatus.IconHandling
{
    /// <summary>
    ///     Create icons.
    /// </summary>
    internal class IconHandler : IIconHandlerInterface<IconEventArgs>, IDisposable
    {
        private const    int               PenWidth      = 48;
        private readonly Rectangle         _boundaries   = new Rectangle(32, 32, 192, 192);
        private readonly AngleCalculations _calculations = new AngleCalculations();
        private readonly Timer             _chargeTimer  = new Timer();

        private readonly Bitmap _iconBitmap = new Bitmap(256, 256);
        private          Icon   _generatedIcon;
        private          bool   _isCharging;

        private float _percentage;
        private bool  _showAnimation;

        public IconHandler()
        {
            _chargeTimer.Elapsed  += ChargeTimer_Elapsed;
            _chargeTimer.Interval =  new TimeSpan(0, 0, 1).TotalMilliseconds;
        }

        public bool ShowChargingAnimation
        {
            get => _showAnimation;
            set
            {
                _showAnimation = IsCharging && value;

                if (_showAnimation)
                {
                    _chargeTimer.Start();
                }
                else
                {
                    _chargeTimer.Stop();
                    Update();
                }
            }
        }

        public float Percentage
        {
            private get => _percentage;
            set
            {
                if (value < 0 || value > 100) throw new PropertyOutOfRangeException();

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

                if (!_isCharging) ShowChargingAnimation = false;

                if (!ShowChargingAnimation) Update();
            }
        }

        public event EventHandler<IconEventArgs> OnUpdate;

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

            OnUpdate?.Invoke(this, new IconEventArgs(_generatedIcon));
        }

        private IntPtr Draw()
        {
            Graphics graphic = Graphics.FromImage(_iconBitmap);

            _calculations.Percentage = Percentage;

            DrawBattery(graphic);
            if (IsCharging) DrawChargingStep(graphic);

            return _iconBitmap.GetHicon();
        }

        private void DrawBattery(Graphics graphic)
        {
            graphic.Clear(Color.Transparent);
            graphic.DrawArc(new Pen(Color.White, PenWidth),
                            _boundaries,
                            _calculations.Start,
                            _calculations.End);
        }

        private void DrawChargingStep(Graphics graphic)
        {
            graphic.DrawArc(new Pen(Color.FromArgb(150, 255, 255, 255), PenWidth),
                            _boundaries,
                            _calculations.Start2,
                            _calculations.End2(ShowChargingAnimation));
        }

        private static void DestroyIcon(Icon icon)
        {
            if (icon != null) DestroyIcon(icon.Handle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private void ChargeTimer_Elapsed(object sender, ElapsedEventArgs e) => Update();
    }
}