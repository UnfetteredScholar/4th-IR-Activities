using System;

namespace _4th_IR.Image_Classification.Classification.Exceptions
{
    public class classificationException : Exception
    {
        public classificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
