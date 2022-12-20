using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SentimentAnalysis.Exceptions;

namespace SentimentAnalysis.DeBERTA
{
    public class SentimentAnalyzerDeBERTA
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
            public double score { get; set; }
        }

        private HttpClient client = null;

        public SentimentAnalyzerDeBERTA(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-sentiment-analysis-deberta.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string, double>> Analyze(string sentence)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var requestContent = new { sentence = sentence };
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

                string resquestUri = "https://text-sentiment-analysis-deberta.ai-sandbox.4th-ir.io/api/v1/sentence";
                response = await client.PostAsync(resquestUri, stringContent);

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent[] responseContent = JsonConvert.DeserializeObject<ResponseContent[]>(r);

                return new Tuple<string, double>(responseContent[0].label, responseContent[0].score);

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

                throw new SentimentAnalysisException(message, ex);
            }
        }
    }


}
