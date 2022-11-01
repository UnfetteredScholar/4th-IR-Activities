using System;

namespace _4thIR.TextSpeechGen.TextToSpeech.Exceptions
{
    public class TextToSpeechException : Exception
    {
        public TextToSpeechException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
