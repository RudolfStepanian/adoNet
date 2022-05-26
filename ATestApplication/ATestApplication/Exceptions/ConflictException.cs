using System;
using System.Runtime.Serialization;

namespace ATestApplication.Exceptions
{
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ConflictException(string errorMessage) : base(errorMessage) { }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
