using System;

namespace TextTranslation.Exceptions
{
    public class TextTranslationException : Exception
    {
        public TextTranslationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
