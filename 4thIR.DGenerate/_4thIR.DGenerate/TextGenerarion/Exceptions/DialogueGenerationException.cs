using System;

namespace _4thIR.DGenerate.TextGenerarion.Exceptions
{
    public class DialogueGenerationException : Exception
    {
        public DialogueGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
