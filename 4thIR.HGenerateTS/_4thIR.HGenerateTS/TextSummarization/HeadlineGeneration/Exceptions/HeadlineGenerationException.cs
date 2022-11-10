using System;

namespace TextSummarization.HeadlineGeneration.Exceptions
{
    public class HeadlineGenerationException : Exception
    {
        public HeadlineGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
