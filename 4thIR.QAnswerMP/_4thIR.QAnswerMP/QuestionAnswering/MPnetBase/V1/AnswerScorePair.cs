

namespace QuestionAnswering.MPnetBase.V1
{
    public class AnswerScorePair
    {
        public AnswerScorePair(string answer, double score)
        {
            Answer = answer;
            Score = score;
        }

        public string Answer { get; set; }
        public double Score { get; set; }
    }
}
