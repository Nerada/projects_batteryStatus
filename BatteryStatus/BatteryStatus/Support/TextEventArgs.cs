//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.Support.TextEventArgs.cs
// Created on: 2019111
//-----------------------------------------------

namespace BatteryStatus.Support
{
    internal class TextEventArgs
    {
        public TextEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}