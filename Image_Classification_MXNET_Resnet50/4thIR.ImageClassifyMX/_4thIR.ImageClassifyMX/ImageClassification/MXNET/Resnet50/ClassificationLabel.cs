namespace ImageClassification.MXNET.Resnet50
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

        /// <summary>
        /// The label or description of the image
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// The probability or accuracy of the label
        /// </summary>
        public double probability { get; set; }
    }
}
