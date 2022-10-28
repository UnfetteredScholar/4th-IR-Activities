using System;

namespace _4thIR.TextGenerateOG.TextGeneration.Exceptions
{
    public class TextGenerationException : Exception
    {
        public TextGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
