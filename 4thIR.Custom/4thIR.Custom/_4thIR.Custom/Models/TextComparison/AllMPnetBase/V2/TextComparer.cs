using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TextComparison.Exceptions;

namespace TextComparison.AllMPnetBase.V2
{
    public class TextComparer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string[] results { get; set; }
        }

        private HttpClient _client = null;

        public TextComparer(HttpClient client)
        {
            _client = client;
        }

        public async Task<string[]> CompareText(string sourceText, string[] sentenceTexts)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { source = sourceText, sentences = sentenceTexts };

                string requestUri = "https://text-comparison-all-mpnet-base.ai-sandbox.4th-ir.io/api/v1/compare";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContents = JsonSerializer.Deserialize<ResponseContent>(r);

                return responseContents.results;
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

                throw new TextComparisonException(message, ex);
            }
        }
    }
}
