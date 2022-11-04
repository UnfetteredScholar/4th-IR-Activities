using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using _4thIR.TextClassifyB.TextClassification.Exceptions;

namespace _4thIR.TextClassifyB.TextClassification.BART
{
    public class TextClassifierBART
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public TextClassifierBART()
        {
            _client.BaseAddress = new Uri("https://text-classification-bart.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> ClassifyText(string inputText)
        {
            var requestContent = new { text = inputText };


            string requestUri = "/api/v1/classify";
            var response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

            try
            {
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

                throw new TextClassificationException(message, ex);
            }
        }
    }
}
