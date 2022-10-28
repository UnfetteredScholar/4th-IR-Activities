using System;

namespace _4thIR.TextGenerateG2.TextGeneration.Exceptions
{
    public class TextGenerationException : Exception
    {
        public TextGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
