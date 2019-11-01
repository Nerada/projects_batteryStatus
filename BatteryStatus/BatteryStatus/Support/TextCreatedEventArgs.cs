using System.Drawing;

namespace BatteryStatus.Support
{
    internal class TextCreatedEventArgs
    {
        public TextCreatedEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}