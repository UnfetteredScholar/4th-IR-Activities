using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using TextSummarization.Exceptions;

namespace TextSummarization.Fairseq
{
    public class TextSummarizerFairseq
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string summary { get; set; }
            public string model { get; set; }

        }

        private HttpClient client = null;

        public TextSummarizerFairseq(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-summarization-fairseq.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> SummarizeText(string article)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { article = article };

                string requestUri = "https://text-summarization-fairseq.ai-sandbox.4th-ir.io/api/v1/sentence";
                response = await client.PostAsJsonAsync(requestUri,requestContent);

                response.EnsureSuccessStatusCode();

                ResponseContent[] responseContent = await response.Content.ReadFromJsonAsync<ResponseContent[]>();

                return responseContent[0].summary;
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

                throw new TextSummarizationException(message, ex);
            }

        }
    }
}
