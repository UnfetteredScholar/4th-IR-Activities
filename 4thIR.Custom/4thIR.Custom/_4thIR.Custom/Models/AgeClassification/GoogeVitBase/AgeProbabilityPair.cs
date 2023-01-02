namespace AgeClassification.GoogeVitBase
{
    public class AgeProbabilityPair
    {
        public AgeProbabilityPair(string ageRange, double probability)
        {
            AgeRange = ageRange;
            Probability = probability;

        }

        /// <summary>
        /// The possible age range
        /// </summary>
        public string AgeRange { get; set; }
        /// <summary>
        /// The probability or degree of accuracy
        /// </summary>
        public double Probability { get; set; }

    }
}
