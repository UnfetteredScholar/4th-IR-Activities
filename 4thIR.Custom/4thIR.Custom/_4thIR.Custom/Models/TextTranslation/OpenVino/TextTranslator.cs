using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using TextTranslation.Exceptions;

namespace TextTranslation.OpenVino
{
    public enum TransationType { English_To_Deutsch, Deutsch_To_English, Russian_To_English, English_To_Russian };

    

    public class TextTranslator
    {
        private class Tanslation
        {
            public Tanslation()
            {

            }

            public string original_sentence { get; set; }
            public string translated_sentence { get; set; }
        }

        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public Tanslation[] results { get; set; }
        }




        private static readonly HttpClient _client = new HttpClient();

        public TextTranslator()
        {
            _client.BaseAddress = new Uri("https://text-translation-openvino.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TranslatedText[]> TranslateText(string[] texts, TransationType transationType)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                string conversion;

                switch (transationType)
                {
                    case TransationType.English_To_Deutsch:
                        conversion = "english_deutschn";
                        break;
                    case TransationType.Deutsch_To_English:
                        conversion = "deutschn_english ";
                        break;
                    case TransationType.English_To_Russian:
                        conversion = "english_russian ";
                        break;
                    case TransationType.Russian_To_English:
                        conversion = "russian_english ";
                        break;
                    default:
                        conversion = "english_deutschn";
                        break;
                }

                var requestContent = new { translation_type = conversion, sentences = texts };

                string requestUri = "/api/v1/translate/";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                TranslatedText[] translatedTexts = new TranslatedText[responseContent.results.Length];

                for (int i = 0; i < responseContent.results.Length; i++)
                    translatedTexts[i] = new TranslatedText(responseContent.results[i].original_sentence, responseContent.results[i].translated_sentence);

                return translatedTexts;
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
                    message = "Error: Unable to complete operation";
                }

                throw new TextTranslationException(message, ex);
            }

        }

    }
}
