using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TextTranslation.Exceptions;

namespace TextTranslation.Transformers
{
    public enum Language { English, French, Romanian, German };

    public class TextTranslator
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string original_text { get; set; }
            public string conversion_text { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public TextTranslator()
        {
            _client.BaseAddress = new Uri("https://text-translation-transformers.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<(string originalText, string conversionText)> TranslateText(string text, Language sourceLanguage, Language conversionLanguage)
        {
            var builder = new UriBuilder("https://text-translation-transformers.ai-sandbox.4th-ir.io/predict/sentence");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["source_lang"] = sourceLanguage.ToString();
            query["conversion_lang"] = conversionLanguage.ToString();
            builder.Query = query.ToString();

            var requestContent = new { sentence = text };
            StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = $"https://text-translation-transformers.ai-sandbox.4th-ir.io/predict/sentence?source_lang={sourceLanguage}%20&conversion_lang={conversionLanguage}";


            //var response = await _client.PostAsync(builder.ToString(),stringContent);
            var response = await _client.PostAsync(requestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent[] responseContent = JsonSerializer.Deserialize<ResponseContent[]>(r);

                return (responseContent[0].original_text, responseContent[0].conversion_text);
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "String is too long";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "ML model not found";
                }
                else
                {
                    message = "Error: Unable to translate text";
                }

                throw new TextTranslationException(message, ex);
            }
        }

    }
}
