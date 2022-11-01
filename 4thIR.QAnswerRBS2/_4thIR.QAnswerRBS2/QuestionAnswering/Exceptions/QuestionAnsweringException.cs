using System;


namespace _4thIR.QAnswerRBS2.QuestionAnswering.Exceptions
{
    public class QuestionAnsweringException : Exception
    {
        public QuestionAnsweringException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
