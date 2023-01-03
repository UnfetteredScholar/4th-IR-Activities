using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using TextSummarization.Exceptions;

namespace TextSummarization.MeetingSummary
{
    public class TextSummarizerMS
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string summarized_text { get; set; }
        }

        private HttpClient _client = null;

        public TextSummarizerMS(HttpClient client)
        {
            _client = client;
            //_client.BaseAddress = new Uri("https://text-summarization-meeting-summary-facebook.ai-sandbox.4th-ir.io");
            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> SummarizeText(string articleText)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var requestContent = new { text = articleText };

                StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

                string requestUri = "https://text-summarization-meeting-summary-facebook.ai-sandbox.4th-ir.io/api/v1/article";
                response = await _client.PostAsync(requestUri, stringContent);

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent[] responseContent = JsonSerializer.Deserialize<ResponseContent[]>(r);

                return responseContent[0].summarized_text;
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

                throw new TextSummarizationException(message, ex);
            }
        }
    }
}
