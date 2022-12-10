namespace PartOfScpeech.Tagging.Flair
{
    /// <summary>
    /// Stores a Word and its Part of Speech
    /// </summary>
    public class WordTag
    {
        /// <summary>
        /// Creates a new instance of the WordTag class
        /// </summary>
        public WordTag()
        {

        }

        /// <summary>
        /// The word
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// The Part of speech
        /// </summary>
        public string value { get; set; }
    }
}
