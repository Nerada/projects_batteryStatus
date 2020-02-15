// -----------------------------------------------
//     Author: Ramon Bollen
//       File: BatteryStatus.TextEventArgs.cs
// Created on: 20191101
// -----------------------------------------------

namespace BatteryStatus.Support
{
    internal class TextEventArgs
    {
        public TextEventArgs(string text) => Text = text;

        public string Text { get; }
    }
}