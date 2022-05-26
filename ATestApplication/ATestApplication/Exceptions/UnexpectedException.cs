using System;
using System.Runtime.Serialization;

namespace ATestApplication.Exceptions
{
    [Serializable]
    public class UnexpectedException : Exception
    {
        public UnexpectedException()
        {
        }

        public UnexpectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public UnexpectedException(string errorMessage) : base(errorMessage) { }

        protected UnexpectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
