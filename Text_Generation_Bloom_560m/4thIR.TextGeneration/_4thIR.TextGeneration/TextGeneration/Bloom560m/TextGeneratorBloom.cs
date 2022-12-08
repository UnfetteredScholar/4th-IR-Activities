using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using TextGeneration.Exceptions;

namespace TextGeneration.Bloom560m
{
    public class TextGeneratorBloom
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string sentence { get; set; }
        }

        private HttpClient _client = null;

        public TextGeneratorBloom(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GenerateText(string sourceText)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { sentence = sourceText };

                string requestUri = "https://text-generation-bloom-560m.ai-sandbox.4th-ir.io/api/v1/classify";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                ResponseContent[] responseContents = await response.Content.ReadFromJsonAsync<ResponseContent[]>();

                return responseContents[0].sentence;
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "String too long";
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
