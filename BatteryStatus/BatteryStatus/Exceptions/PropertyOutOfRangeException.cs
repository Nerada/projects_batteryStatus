// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.PropertyOutOfRangeException.cs
// Created on: 20201207
// -----------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

[assembly: InternalsVisibleTo("BatteryStatus.tests")]

namespace BatteryStatus.Exceptions
{
    [Serializable]
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

        protected PropertyOutOfRangeException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }
    }
}