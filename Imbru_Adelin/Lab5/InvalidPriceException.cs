using System;
using System.Runtime.Serialization;

namespace Lab5
{
    [Serializable]
    internal class InvalidPriceException : Exception
    {
        public InvalidPriceException()
        {
        }

        public InvalidPriceException(string? message) : base(message)
        {
        }

        public InvalidPriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidPriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}