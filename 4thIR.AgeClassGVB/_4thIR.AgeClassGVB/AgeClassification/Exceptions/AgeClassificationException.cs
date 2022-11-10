using System;

namespace AgeClassification.Exceptions
{
    public class AgeClassificationException : Exception
    {
        public AgeClassificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
