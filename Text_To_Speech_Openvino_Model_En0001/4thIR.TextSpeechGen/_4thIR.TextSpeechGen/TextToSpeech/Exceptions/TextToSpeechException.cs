using System;

namespace TextToSpeech.Exceptions
{
    public class TextToSpeechException : Exception
    {
        public TextToSpeechException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
