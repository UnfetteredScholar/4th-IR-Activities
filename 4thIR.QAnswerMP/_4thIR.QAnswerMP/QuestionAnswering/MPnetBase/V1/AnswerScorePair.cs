

namespace QuestionAnswering.MPnetBase.V1
{
    /// <summary>
    /// Stores the answer to a question and its accuracy
    /// </summary>
    public class AnswerScorePair
    {
        /// <summary>
        /// Creates an instance of the AnswerScorePair class
        /// </summary>
        /// <param name="answer">The answer</param>
        /// <param name="score">The accuracy</param>
        public AnswerScorePair(string answer, double score)
        {
            Answer = answer;
            Score = score;
        }

        /// <summary>
        /// The Answer to the question
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// The accuracy of the answer
        /// </summary>
        public double Score { get; set; }
    }
}
