using System;

namespace SentenceCompletion.Exceptions
{
    public class SentenceCompletionException : Exception
    {
        public SentenceCompletionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
