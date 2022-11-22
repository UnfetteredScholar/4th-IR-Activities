using System;

namespace ObjectDetection.Exceptions
{
    public class ObjectDetectionException : Exception
    {
        public ObjectDetectionException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
