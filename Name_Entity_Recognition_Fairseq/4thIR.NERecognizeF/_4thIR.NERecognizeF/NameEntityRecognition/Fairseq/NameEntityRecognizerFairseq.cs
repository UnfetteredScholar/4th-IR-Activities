using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NameEntityRecognition.Exceptions;

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

        private static readonly HttpClient client = new HttpClient();

        public NameEntityRecognizerFairseq()
        {
            client.BaseAddress = new Uri("https://text-name-entity-recognition-flair-2.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TextValuePair[]> RecognizeNameEntity(string sentence)
        {
            RequestContent requestContent = new RequestContent(sentence);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/sentence";
            var response = await client.PostAsync(requestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                TextValuePair[] result = JsonConvert.DeserializeObject<TextValuePair[]>(r);

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
