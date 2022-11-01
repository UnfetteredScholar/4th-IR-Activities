using System;

namespace _4thIR.TSummarizeFB.TextSummarization.Exceptions
{
    public class TextSummarizationException : Exception
    {
        public TextSummarizationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
