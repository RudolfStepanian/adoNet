using System;
using System.Runtime.Serialization;

namespace ATestApplication.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadRequestException(string errorMessage) : base(errorMessage) { }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
