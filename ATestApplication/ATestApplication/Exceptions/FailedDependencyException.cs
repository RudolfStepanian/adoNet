using System;
using System.Runtime.Serialization;

namespace ATestApplication.Exceptions
{
    [Serializable]
    public class FailedDependencyException : Exception
    {
        public FailedDependencyException()
        {
        }

        public FailedDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public FailedDependencyException(string errorMessage) : base(errorMessage) { }

        protected FailedDependencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
