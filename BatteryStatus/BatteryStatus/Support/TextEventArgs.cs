// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.TextEventArgs.cs
// Created on: 20201207
// -----------------------------------------------

namespace BatteryStatus.Support
{
    internal class TextEventArgs
    {
        public TextEventArgs(string text) => Text = text;

        public string Text { get; }
    }
}