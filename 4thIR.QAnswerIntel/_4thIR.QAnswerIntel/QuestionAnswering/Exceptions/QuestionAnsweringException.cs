using System;


namespace _4thIR.QAnswerIntel.QuestionAnswering.Exceptions
{
    public class QuestionAnsweringException : Exception
    {
        public QuestionAnsweringException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
