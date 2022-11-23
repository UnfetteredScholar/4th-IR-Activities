using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TextClassification.Exceptions;

namespace TextClassification.Facebook.Bart
{
    public class TextClassifierFB
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public TextClassificationAnswer answer { get; set; }
            public string model { get; set; }
        }

        private class TextClassificationAnswer
        {
            public TextClassificationAnswer()
            {

            }

            public string sequence { get; set; }
            public string[] labels { get; set; }
            public double[] scores { get; set; } //
        }

        private static readonly HttpClient _client = new HttpClient();

        public TextClassifierFB()
        {
            _client.BaseAddress = new Uri("https://text-classification-bart-large-mnli.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<(string[] labels, double[] scores)> ClassifyText(string sentence, string labelList)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { text = sentence, labels = labelList };


                string requestUri = "/api/v1/classify";
                response = await _client.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json"));


                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

                var answer = responseContent.answer;

                return new(answer.labels, answer.scores);
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
