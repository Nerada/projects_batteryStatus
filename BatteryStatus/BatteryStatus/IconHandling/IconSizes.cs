// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.IconSizes.cs
// Created on: 20210208
// -----------------------------------------------

namespace BatteryStatus.IconHandling
{
    public struct IconSizes
    {
        private const double Scale = 1;

        public const int BitmapSize      = (int)(256 * Scale); //256
        public const int PenWidthHighRes = (int)(48  * Scale);
        public const int PenWidthLowRes  = (int)(64  * Scale);

        public const int BatteryBoundaryPosition = BitmapSize / 8; //32
        public const int BatteryBoundarySize     = BitmapSize - (BatteryBoundaryPosition * 2); //192

        public const int AwakeBoundaryPosition = (BatteryBoundaryPosition * 3) - 2; //94
        public const int AwakeBoundarySize     = (BatteryBoundarySize     / 3) + 4; //68
    }
}