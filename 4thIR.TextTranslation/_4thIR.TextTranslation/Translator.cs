using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;


namespace TextTranslation
{
    /// <summary>
    /// Supported Languages for translation
    /// </summary>
    public enum Language { Arabic, Czech, German, English, Spanish, Estonian, Finnish, French, Gujarati, Hindi, Italian, Japanese, Kazakh, Korean, Lithuanian, Latvian, Burmese, Nepali, Dutch, Romanian, Russian, Sinhala, Turkish, Vietnamese, Chinese, Afrikaans, Azerbaijani, Bengali, Persian, Hebrew, Croatian, Indonesian, Georgian, Khmer, Macedonian, Malayalam, Mongolian, Marathi, Polish, Pashto, Portuguese, Swedish, Swahili, Tamil, Telugu, Thai, Tagalog, Ukrainian, Urdu, Xhosa, Galician, Slovene };


    /// <summary>
    /// Provides functionality for translating text form source language to target language.
    /// </summary>
    public class Translator
    {
        private class RequestContent
        {
            public string sentence { get; set; }

            public RequestContent(string request)
            {
                sentence = request;
            }
        }

        private class ResponseContent
        {
            public string original_text { get; set; }
            public string conversion_text { get; set; }

            public ResponseContent()
            {
                original_text = null;
                conversion_text = null;
            }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Constructor for Translator Class
        /// </summary>
        public Translator()
        {
            client.BaseAddress = new Uri("https://text-translation-fairseq-1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Translates text from source language to specified language
        /// </summary>
        /// <param name="sentence">Text to be translated</param>
        /// <param name="sourceLanguage">Language of text to be translated</param>
        /// <param name="conversionLanguage">Target language for translation</param>
        /// <exception cref="Exception">Thrown when translation fails</exception>
        /// <returns>Returns the translated text </returns>
        public async Task<string> TranslateText(string sentence, Language sourceLanguage, Language conversionLanguage)
        {
            RequestContent request = new RequestContent(sentence);

            string requestUri = $"/api/v1/sentence?source_lang={sourceLanguage}&conversion_lang={conversionLanguage}";

            var requestJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, requestJson);


            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] param = { '[', ']' };
                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(param));

                return responseContent?.conversion_text != null ? responseContent.conversion_text : " ";

            }
            catch(HttpRequestException ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message= "BAD REQUEST: string too long.";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "INTERNAL SERVER ERROR: ML model not found.";
                }
                else
                {
                    message = "ERROR: TRANSLATOR FAILED.";
                }

                throw new Exception(message,ex);
            }


        }

    }
}
