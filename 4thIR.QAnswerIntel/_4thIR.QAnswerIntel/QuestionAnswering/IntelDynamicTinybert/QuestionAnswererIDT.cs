using System;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using _4thIR.QAnswerIntel.QuestionAnswering.Exceptions;

namespace _4thIR.QAnswerIntel.QuestionAnswering.IntelDynamicTinybert
{
    public class QuestionAnswererIDT
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
            public string model { get; set; }
        }

        private class RequestContent
        {
            public RequestContent(string textContext, string question) => (text_context, this.question) = (textContext, question);

            public string text_context { get; set; }
            public string question { get; set; }

        }

        private static readonly HttpClient _client = new HttpClient();

        public QuestionAnswererIDT()
        {
            _client.BaseAddress = new Uri("https://text-question-answer-intel-dynamic-tinybert.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> AnswerQuestion(string context, string question)
        {

            RequestContent requestContent = new RequestContent(context, question);


            StringContent questionContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/predict";
            var response = await _client.PostAsync(requestUri, questionContent);

            try
            {
                response.EnsureSuccessStatusCode();

                var r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return responseContent.answer;
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
