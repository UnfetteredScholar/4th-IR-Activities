using System;

namespace _4thIR.SCompletePosT.SentenceCompletion.Exceptions
{
    public class SentenceCompletionException : Exception
    {
        public SentenceCompletionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
