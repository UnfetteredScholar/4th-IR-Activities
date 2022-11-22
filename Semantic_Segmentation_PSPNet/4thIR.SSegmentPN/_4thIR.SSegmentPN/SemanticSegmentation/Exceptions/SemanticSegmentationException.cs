using System;

namespace SemanticSegmentation.Exceptions
{
    public class SemanticSegmentationException : Exception
    {
        public SemanticSegmentationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
