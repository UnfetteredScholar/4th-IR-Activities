using System;

namespace TextComparison.Exceptions
{
    public class TextComparisonException : Exception
    {
        public TextComparisonException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
