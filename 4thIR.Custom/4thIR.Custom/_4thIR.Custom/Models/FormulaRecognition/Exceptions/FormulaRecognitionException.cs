using System;

namespace FormulaRecognition.Exceptions
{
    public class FormulaRecognitionException:Exception
    {
        public FormulaRecognitionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
