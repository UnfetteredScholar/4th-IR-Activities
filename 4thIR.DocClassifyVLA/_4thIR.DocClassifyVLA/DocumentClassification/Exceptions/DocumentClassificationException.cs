using System;

namespace _4thIR.DocClassifyVLA.DocumentClassification.Exceptions
{
    public class DocumentClassificationException : Exception
    {
        public DocumentClassificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
