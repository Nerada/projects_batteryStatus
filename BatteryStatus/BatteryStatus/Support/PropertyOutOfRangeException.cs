using System;

namespace BatteryStatus.Support
{
    public class PropertyOutOfRangeException : Exception
    {
        public PropertyOutOfRangeException()
        {
        }

        public PropertyOutOfRangeException(string message) : base(message)
        {
        }

        public PropertyOutOfRangeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}