using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using TextComparison.Exceptions;

namespace TextComparison.AllMiniLM
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




        private static readonly HttpClient _client = new HttpClient();

        public TextComparer()
        {
            _client.BaseAddress = new Uri("https://text-comparison-all-minilm.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string[]> CompareText(string[] texts)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                StringBuilder requestContent = new StringBuilder();
                requestContent.Append($"[\"{texts[0]}\"");

                for (int i = 1; i < texts.Length; i++)
                    requestContent.Append($",\"{texts[i]}\"");

                requestContent.Append("]");


                string requestUri = "/api/v1/compare";
                response = await _client.PostAsync(requestUri, new StringContent(requestContent.ToString(), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);


                return responseContent.results;
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

                throw new TextTranslationException(message, ex);
            }

        }


    }
}
