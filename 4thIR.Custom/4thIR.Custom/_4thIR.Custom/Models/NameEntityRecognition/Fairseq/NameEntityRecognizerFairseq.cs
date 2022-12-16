using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using NameEntityRecognition.Exceptions;
using System.Text.Json;

namespace NameEntityRecognition.Fairseq
{

    public class NameEntityRecognizerFairseq
    {
        private class RequestContent
        {
            public RequestContent(string sentence)
            {
                this.sentence = sentence;
            }

            public string sentence { get; set; }
        }

        private HttpClient client = null;

        public NameEntityRecognizerFairseq(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-name-entity-recognition-flair-2.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TextValuePair[]> RecognizeNameEntity(string sentence)
        {
            HttpResponseMessage response=new HttpResponseMessage();

            try
            {
                var requestContent = new { sentence = sentence };

                string requestUri = "https://text-name-entity-recognition-flair-2.ai-sandbox.4th-ir.io/api/v1/sentence";
                response = await client.PostAsJsonAsync(requestUri, requestContent);

                response.EnsureSuccessStatusCode();

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                TextValuePair[] result = await response.Content.ReadFromJsonAsync<TextValuePair[]>(jsonOptions);

                return result;
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

                throw new NameEntityRecognitionException(message, ex);
            }
        }

    }
}
