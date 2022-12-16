using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using TextGeneration.Exceptions;

namespace TextGeneration.Fairseq
{
    public class TextGeneratorFairseq
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

        private HttpClient client = null;

        public TextGeneratorFairseq(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-generation-fairseq-1.ai-sandbox.4th-ir.io");
            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string, string>> GenerateText(string sentence)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { sentence = sentence };

                string requestUri = "https://text-generation-fairseq-1.ai-sandbox.4th-ir.io/api/v1/sentence";
                response = await client.PostAsJsonAsync(requestUri, requestContent);

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] chars = { '[', ']' };

                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(chars));

                return new Tuple<string, string>(responseContent.generated_text_1, responseContent.generated_text_2);
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
