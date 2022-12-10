namespace TextTranslation.OpenVino
{
    public class TranslatedText
    {
        public TranslatedText(string originaltext, string translatedText)
        {
            OriginalSentence = originaltext;
            TranslatedSentence = translatedText;
        }

        public string OriginalSentence { get; set; }
        public string TranslatedSentence { get; set; }
    }
}
