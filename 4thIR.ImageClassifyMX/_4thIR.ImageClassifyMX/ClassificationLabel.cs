
namespace ImageClassification
{
    /// <summary>
    /// Contains a classification label and its probability/ accuracy
    /// </summary>
    public class ClassificationLabel
    {
        /// <summary>
        /// Creates a new instance of the ClassificationLabel class
        /// </summary>
        public ClassificationLabel()
        {

        }

        public string label { get; set; }
        public double probability { get; set; }
    }
}
