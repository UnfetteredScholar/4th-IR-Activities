using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace QuestionAnswerCUAD
{
    /// <summary>
    /// Provides methods for answering questions based on context
    /// </summary>
    public class QuestionAnswererCUAD
    {

        private class RequestContent
        {
            public RequestContent(string question, string context)
            {
                this.question = question;
                this.context = context;
            }

            public string question { get; set; }
            public string context { get; set; }
        }

        private class ResponseContent
        {
            private ResponseContent()
            {

            }

            public string answer { get; set; }
            public double score { get; set; }

        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates a new instance of the QuestionAnswererCUAD class
        /// </summary>
        public QuestionAnswererCUAD()
        {
            client.BaseAddress = new Uri("https://text-question-answer-roberta-base-squad2.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.Timeout = TimeSpan.FromSeconds(timeout);
        }



        /// <summary>
        /// Answers a question based on context
        /// </summary>
        /// <param name="question">Question to be asked</param>
        /// <param name="context">Context of the question</param>
        /// <returns>Returns a tuple containing the answer to the question and the score</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<string, double>> AnswerQuestion(string question, string context)
        {
            RequestContent requestContent = new RequestContent(question, context);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/question";
            var response = await client.PostAsync(requestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] chars = { '[', ']' };

                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(chars));

                return new Tuple<string, double>(responseContent.answer, responseContent.score);
            }
            catch (HttpRequestException ex)
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
