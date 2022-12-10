using System;

namespace SentimentAnalysis.Exceptions
{
    public class SentimentAnalysisException : Exception
    {
        public SentimentAnalysisException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
