using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TextClassification.Exceptions;

namespace TextClassification.ZeroShot.Roberta
{
    public class LabelScorePair
    {
        public LabelScorePair()
        {

        }

        public string Label { get; set; }
        public double Score { get; set; }
    }

    public class TextClassifierZSR
    {
        private HttpClient _client = null;

        public TextClassifierZSR(HttpClient client)
        {
            _client = client;
        }

        public async Task<LabelScorePair[]> ClassifyText(string text, string[] labels)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent=new {sentence=text, label=labels};

                string requestUri = "https://text-zero-shot-classification-roberta.ai-sandbox.4th-ir.io/api/v1/classify";
                response = await _client.PostAsJsonAsync(requestUri, requestContent);

                response.EnsureSuccessStatusCode();

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                LabelScorePair[] labelScorePairs = await response.Content.ReadFromJsonAsync<LabelScorePair[]>(jsonOptions);

                return labelScorePairs;
            }
            catch(Exception ex)
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

                throw new TextClassificationException(message, ex);

            }
        }

    }
}
