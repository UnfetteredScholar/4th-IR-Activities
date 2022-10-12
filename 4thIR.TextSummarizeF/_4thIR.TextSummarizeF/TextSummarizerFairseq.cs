using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TextSummarizationFairseq
{
    public class TextSummarizerFairseq
    {
        private class RequestContent
        {
            public RequestContent(string article)
            {
                this.article = article;
            }

            public string article { get; set; }

        }

        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string summary { get; set; }
            public string model { get; set; }

        }

        private static readonly HttpClient client=new HttpClient();

        public TextSummarizerFairseq()
        {
            client.BaseAddress = new Uri("https://text-summarization-fairseq.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string,string>> SummarizeText(string article)
        {
            RequestContent requestContent = new RequestContent(article);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            
            string requestUri = "/api/v1/sentence";
            var response = await client.PostAsync(requestUri,stringContent);


            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] chars = { '[', ']' };

                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(chars));

                return new Tuple<string, string>(responseContent.summary, responseContent.model);
            }
            catch(HttpRequestException ex)
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

                throw new Exception(message, ex);
            }

        }
    }
}
