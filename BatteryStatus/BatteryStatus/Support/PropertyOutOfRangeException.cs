//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: BatteryStatus.Support.PropertyOutOfRangeException.cs
// Created on: 20191030
//-----------------------------------------------

using System;

namespace BatteryStatus.Support
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