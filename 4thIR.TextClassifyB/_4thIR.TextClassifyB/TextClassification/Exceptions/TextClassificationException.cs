using System;

namespace TextClassification.Exceptions
{
    public class TextClassificationException : Exception
    {
        public TextClassificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
