using System.Drawing;

namespace BatteryStatus.Support
{
    internal class IconCreatedEventArgs
    {
        public IconCreatedEventArgs(Icon icon)
        {
            Icon = icon;
        }

        public Icon Icon { get; }
    }
}