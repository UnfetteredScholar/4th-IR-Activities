using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TextSummarization.Exceptions;

namespace TextSummarization.Bert2Bert.Small
{
    public class TextSummarizerB2BS
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
        }

        private HttpClient _client = null;

        public TextSummarizerB2BS(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SummarizeText(string text)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { text = text};

                string requestUri = "https://text-summarization-bert2bert-small.ai-sandbox.4th-ir.io/api/v1/classify";
                response = await _client.PostAsJsonAsync(requestUri, requestContent);

                response.EnsureSuccessStatusCode();

                ResponseContent[] responseContent = await response.Content.ReadFromJsonAsync<ResponseContent[]>();

                return responseContent[0].answer;
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

                throw new TextSummarizationException(message, ex);

            }
        }
    }
}
