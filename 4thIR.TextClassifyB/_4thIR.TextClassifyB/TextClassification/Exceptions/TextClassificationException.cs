using System;

namespace _4thIR.TextClassifyB.TextClassification.Exceptions
{
    public class TextClassificationException : Exception
    {
        public TextClassificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
