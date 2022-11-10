using System;

namespace TextGeneration.Exceptions
{
    public class TextGenerationException : Exception
    {
        public TextGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
