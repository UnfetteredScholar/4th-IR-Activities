using System;


namespace QuestionAnswering.Exceptions
{
    public class QuestionAnsweringException : Exception
    {
        public QuestionAnsweringException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
