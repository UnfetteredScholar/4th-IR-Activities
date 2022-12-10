using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using QuestionAnswering.Exceptions;

namespace QuestionAnswering.MPnetBase.V1
{
    public class QuestionAnswererM
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
            public double score { get; set; }
        }


        private static readonly HttpClient _client = new HttpClient();

        public QuestionAnswererM()
        {
            _client.BaseAddress = new Uri("https://text-question-answer-mpnet-base-v1.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<AnswerScorePair[]> AnswerQuestion(string questionAsked, string[] context)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { question = questionAsked, content = context };


                string requestUri = "/api/v1/answer";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));


                response.EnsureSuccessStatusCode();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                string r = await response.Content.ReadAsStringAsync();
                AnswerScorePair[] responseContent = JsonSerializer.Deserialize<AnswerScorePair[]>(r, options);


                return responseContent;
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

                throw new QuestionAnsweringException(message, ex);
            }
        }
    }
}
