using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TextTranslation.Exceptions;
using System.ComponentModel;

namespace TextTranslation.MachineTranslation.Transformers2
{
    public enum Language { [Description("Select A Language")] SelectALanguage, English, French, Romanian, German }
    public class TextTranslatorT
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string original_text { get; set; }
            public string conversion_text { get; set; }
        }

        private HttpClient client = null;

        public TextTranslatorT(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-machine-translation-transformers-2.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> TranslateText(string text, Language sourceLanguage, Language conversionLanguage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(text, Encoding.UTF8, "text/plain"), "sentence");


                    string requestUri = $"https://text-machine-translation-transformers-2.ai-sandbox.4th-ir.io/api/v1/translate?source_lang={sourceLanguage}&conversion_lang={conversionLanguage}";
                    response = await client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    ResponseContent[] responseContent = await response.Content.ReadFromJsonAsync<ResponseContent[]>();

                    return responseContent[0].conversion_text;
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "string too long";
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

