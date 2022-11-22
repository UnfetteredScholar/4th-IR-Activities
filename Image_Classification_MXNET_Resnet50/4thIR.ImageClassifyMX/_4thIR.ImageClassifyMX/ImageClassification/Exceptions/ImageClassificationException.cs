using System;

namespace ImageClassification.Exceptions
{
    public class ImageClassificationException:Exception
    {
        public ImageClassificationException(string message, Exception innerException):base(message,innerException)
        {

        }
    }
}
