using System;

namespace SentenceMatching.Exceptions
{
    public class SentenceMatchingException : Exception
    {
        public SentenceMatchingException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
