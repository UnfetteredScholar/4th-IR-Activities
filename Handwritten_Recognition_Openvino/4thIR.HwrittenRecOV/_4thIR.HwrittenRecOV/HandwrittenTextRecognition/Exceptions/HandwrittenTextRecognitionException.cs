using System;

namespace HandwrittenTextRecognition.Exceptions
{
    public class HandwrittenTextRecognitionException:Exception
    {
        public HandwrittenTextRecognitionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
