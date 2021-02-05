// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IconHandler.cs
// Created on: 20201207
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
    internal sealed class IconHandler : IIconHandlerInterface<IconEventArgs>, IDisposable
    {
        private const double Scale = 1;

        private const int BitmapSize = (int)(256 * Scale); //256
        private const int PenWidth   = (int)(48  * Scale);

        private const int BatteryBoundaryPosition = BitmapSize / 8; //32
        private const int BatteryBoundarySize     = BitmapSize - (BatteryBoundaryPosition * 2); //192

        private const int AwakeBoundaryPosition = (BatteryBoundaryPosition * 3) - 2; //94
        private const int AwakeBoundarySize     = (BatteryBoundarySize     / 3) + 4; //68

        private readonly Rectangle _batteryBoundaries   = new(BatteryBoundaryPosition, BatteryBoundaryPosition, BatteryBoundarySize, BatteryBoundarySize);
        private readonly Rectangle _stayAwakeBoundaries = new(AwakeBoundaryPosition, AwakeBoundaryPosition, AwakeBoundarySize, AwakeBoundarySize);

        private readonly AngleCalculations _calculations = new();
        private readonly Timer             _chargeTimer  = new();

        private readonly Bitmap _iconBitmap = new(BitmapSize, BitmapSize);
        private          Icon?  _generatedIcon;
        private          bool   _isCharging;

        private float _percentage;
        private bool  _showAnimation;

        private bool _isDisposed;
        private bool _stayAwake;

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

        public bool StayAwake
        {
            get => _stayAwake;
            set
            {
                _stayAwake = value;
                Update();
            }
        }

        public event EventHandler<IconEventArgs>? OnUpdate;

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            _chargeTimer.Close();
            _iconBitmap.Dispose();

            if (_generatedIcon is not { }) return;

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
            _calculations.Percentage = Percentage;

            using Graphics graphic = Graphics.FromImage(_iconBitmap);

            graphic.Clear(Color.Transparent);

            DrawBatteryIndicator(graphic, _calculations);
            if (IsCharging) DrawChargingStepIndicator(graphic, _calculations);
            if (StayAwake) DrawStayAwakeIndicator(graphic);

            return _iconBitmap.GetHicon();
        }

        private void DrawBatteryIndicator(Graphics graphic, AngleCalculations angleCalculations)
        {
            graphic.DrawArc(new Pen(Color.White, PenWidth),
                            _batteryBoundaries,
                            angleCalculations.Start,
                            angleCalculations.End);
        }

        private void DrawChargingStepIndicator(Graphics graphic, AngleCalculations angleCalculations)
        {
            graphic.DrawArc(new Pen(Color.FromArgb(150, 255, 255, 255), PenWidth),
                            _batteryBoundaries,
                            angleCalculations.Start2,
                            angleCalculations.End2(ShowChargingAnimation));
        }

        private void DrawStayAwakeIndicator(Graphics graphic)
        {
            graphic.DrawArc(new Pen(Color.OrangeRed, _stayAwakeBoundaries.Width),
                            _stayAwakeBoundaries, 0, 360);
        }

        private static void DestroyIcon(Icon? icon)
        {
            if (icon != null) DestroyIcon(icon.Handle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private void ChargeTimer_Elapsed(object sender, ElapsedEventArgs e) => Update();
    }
}