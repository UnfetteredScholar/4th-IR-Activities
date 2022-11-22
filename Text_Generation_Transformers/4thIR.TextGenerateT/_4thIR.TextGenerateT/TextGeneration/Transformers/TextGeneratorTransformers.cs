using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TextGeneration.Exceptions;

namespace TextGeneration.Transformers
{
    public class TextGeneratorTransformers
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

            public string generated_text_1 { get; set; }
            public string generated_text_2 { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        public TextGeneratorTransformers()
        {
            client.BaseAddress = new Uri("https://text-generation-transformers-1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string, string>> GenerateText(string sentence)
        {
            RequestContent requestContent = new RequestContent(sentence);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/sentence";
            var response = await client.PostAsync(requestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent[] responseContent = JsonConvert.DeserializeObject<ResponseContent[]>(r);

                return new Tuple<string, string>(responseContent[0].generated_text_1, responseContent[0].generated_text_2);
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

                throw new TextGenerationException(message, ex);
            }
        }
    }
}
