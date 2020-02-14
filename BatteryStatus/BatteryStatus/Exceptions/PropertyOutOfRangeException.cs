//-----------------------------------------------
//      Author: Ramon Bollen
//       File: BatteryStatus.Exceptions.PropertyOutOfRangeException.cs
// Created on: 2019111
//-----------------------------------------------

using System;

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

        protected PropertyOutOfRangeException(System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}