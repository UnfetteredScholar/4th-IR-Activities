using System;

namespace DocumentClassification.Exceptions
{
    public class DocumentClassificationException : Exception
    {
        public DocumentClassificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
