using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SentimentAnalysis
{
    public class SentimentAnalyzerDeBERTA
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

            public string label { get; set; }
            public double score { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        public SentimentAnalyzerDeBERTA()
        {
            client.BaseAddress = new Uri("https://text-sentiment-analysis-deberta.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string,double>> Analyze(string sentence)
        {
            RequestContent requestContent = new RequestContent(sentence);
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

            string resquestUri = "/api/v1/sentence";
            var response=await client.PostAsync(resquestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent[] responseContent = JsonConvert.DeserializeObject<ResponseContent[]>(r);

                return new Tuple<string, double>(responseContent[0].label, responseContent[0].score);

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
