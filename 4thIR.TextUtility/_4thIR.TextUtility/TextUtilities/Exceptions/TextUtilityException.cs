using System;

namespace _4thIR.TextUtility.TextUtilities.Exceptions
{
    internal class TextUtilityException : Exception
    {
        public TextUtilityException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
