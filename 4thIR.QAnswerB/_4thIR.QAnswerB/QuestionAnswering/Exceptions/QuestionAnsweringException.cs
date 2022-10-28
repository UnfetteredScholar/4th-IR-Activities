using System;


namespace _4thIR.QAnswerB.QuestionAnswering.Exceptions
{
    public class QuestionAnsweringException : Exception
    {
        public QuestionAnsweringException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
