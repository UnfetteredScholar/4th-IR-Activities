using System;

namespace PoseEstimation.Exceptions
{
    public class PoseEstimationException : Exception
    {
        public PoseEstimationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
