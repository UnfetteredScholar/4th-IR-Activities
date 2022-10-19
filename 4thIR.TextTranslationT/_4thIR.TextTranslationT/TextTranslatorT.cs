using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TextTranslation
{
    public enum Language { English, French, Romanian, German }
    public class TextTranslatorT
    {

        private class RequestContent
        {
            public RequestContent(string sentence)
            {
                this.sentence = sentence;
            }

            public string sentence { get; set; }    
        }
             

        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string original_text { get; set; }
            public string conversion_text { get; set; }
        }

        private static readonly HttpClient client=new HttpClient();

        public TextTranslatorT()
        {
            //client.BaseAddress = new Uri("https://text-machine-translation-transformers-2.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> TranslateText(string text,Language sourceLanguage,Language conversionLanguage)
        {
            using(var formData=new MultipartFormDataContent())
            {
                formData.Add(new StringContent(text, Encoding.UTF8, "text/plain"), "sentence");


                string requestUri = $"https://text-machine-translation-transformers-2.ai-sandbox.4th-ir.io/api/v1/translate?source_lang={sourceLanguage}&conversion_lang={conversionLanguage}";
                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ResponseContent[] responseContent = JsonConvert.DeserializeObject<ResponseContent[]>(r);

                    return responseContent[0].conversion_text;
                }
                catch(HttpRequestException ex)
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

                    throw new Exception(message, ex);
                }
            }


        }
    }
}
