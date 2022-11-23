using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using QuestionAnswering.Exceptions;

namespace QuestionAnswering.XLM.RobertaBaseSquad2
{
    public class QuestionAnswererRBS2
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

        public QuestionAnswererRBS2()
        {
            _client.BaseAddress = new Uri("https://text-question-answer-xlm-roberta-squad2.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(string answer, double score)> AnswerQuestion(string questionAsked, string questionContext)
        {
            var requestContent = new { question = questionAsked, context = questionContext };

            StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/question";
            var response = await _client.PostAsync(requestUri, stringContent);

            try
            {
                string r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return new(responseContent.answer, responseContent.score);
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
