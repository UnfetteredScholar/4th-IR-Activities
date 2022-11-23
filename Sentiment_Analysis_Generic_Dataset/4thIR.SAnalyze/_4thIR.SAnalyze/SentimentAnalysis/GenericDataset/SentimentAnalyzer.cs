using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using SentimentAnalysis.Exceptions;

namespace SentimentAnalysis.GenericDataset
{
    public class SentimentAnalyzer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public SentimentAnalyzer()
        {
            _client.BaseAddress = new Uri("https://text-sentiment-analysis-generic-dataset.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> AnalyzeText(string text)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { sentence = text };

                string requestUri = "/api/v1/analyze";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return responseContent.label;
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

                throw new SentimentAnalysisException(message, ex);
            }

        }
    }
}
