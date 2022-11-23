using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using QuestionAnswering.Exceptions;

namespace QuestionAnswering.ImageContext
{
    public class ImageQuestionAnswerer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public double score { get; set; }
            public string answer { get; set; }
            public int start { get; set; }
            public int end { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public ImageQuestionAnswerer()
        {
            _client.BaseAddress = new Uri("https://image-question-answering-layoutlm.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<(string answer, double score, int start, int end)> AnswerQuestion(string imageContextPath, string question)
        {
            imageContextPath = @"" + imageContextPath;
            HttpResponseMessage response = new HttpResponseMessage();



            try
            {
                var builder = new UriBuilder("https://image-question-answering-layoutlm.ai-sandbox.4th-ir.io/api/v1/answer");
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["question"] = question;
                builder.Query = query.ToString();

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(imageContextPath));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    int index = imageContextPath.LastIndexOf('\\') + 1;
                    string fileName = imageContextPath.Substring(index);

                    formData.Add(imageStream, "file", fileName);

                    string requestUri = builder.ToString();
                    response = await _client.PostAsync(requestUri, formData);
                }

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

                return new(responseContent.answer, responseContent.score, responseContent.start, responseContent.end);
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Media type not supported";
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
