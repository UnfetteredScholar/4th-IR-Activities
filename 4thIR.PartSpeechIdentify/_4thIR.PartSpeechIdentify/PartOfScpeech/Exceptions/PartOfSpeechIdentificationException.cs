using System;

namespace PartOfScpeech.Exceptions
{
    public class PartOfSpeechIdentificationException:Exception
    {
        public PartOfSpeechIdentificationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
