using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using QuestionAnswering.Exceptions;

namespace QuestionAnswering.Image.LayoutLMv2
{
    public class DocumentImageQuestionAnswerer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
        }

        private HttpClient _client = null;

        public DocumentImageQuestionAnswerer(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> AnswerQuestion(string path, string question)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    int index = path.LastIndexOf('\\');
                    string fileName = path.Substring(index);

                    formData.Add(imageStream, "file", fileName);

                    formData.Add(new StringContent(question, Encoding.UTF8, "text/plain"), "question");

                    string requestUri = "https://image-question-answer-layoutlm-v2.ai-sandbox.4th-ir.io/api/v1/answer";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ResponseContent[] responseContents = JsonSerializer.Deserialize<ResponseContent[]>(r);

                    return responseContents[0].answer;

                }

            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Image must be jpeg or png format";
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
