using System;

namespace NameEntityRecognition.Exceptions
{
    public class NameEntityRecognitionException:Exception
    {
        public NameEntityRecognitionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
