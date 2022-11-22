namespace NameEntityRecognition.Flair
{
    /// <summary>
    /// Defines a word and its corresponding value
    /// </summary>
    public class TextValuePair
    {
        /// <summary>
        /// Creates a new instance of the TextValuePair class
        /// </summary>
        public TextValuePair()
        {

        }

        /// <summary>
        /// The word
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// The value of the word
        /// </summary>
        public string value { get; set; }
    }
}
