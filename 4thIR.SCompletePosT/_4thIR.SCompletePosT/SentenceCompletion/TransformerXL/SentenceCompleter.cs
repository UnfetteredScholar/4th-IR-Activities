using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SentenceCompletion.Exceptions;

namespace SentenceCompletion.TransformerXL
{
    public class SentenceCompleter
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string text { get; set; }
        }

        private class RequestContent
        {
            public RequestContent(string sentence) => this.sentence = sentence;

            public string sentence { get; set; }

        }

        private static readonly HttpClient _client = new HttpClient();

        public SentenceCompleter()
        {
            _client.BaseAddress = new Uri("https://text-part-of-speech-tagging-transformer-xl.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        /// <summary>
        /// Completes the input text
        /// </summary>
        /// <param name="sentence">The original text to complete</param>
        /// <returns>Tuple(AddedText, CompletedText)</returns>
        /// <exception cref="TextGenerationException"></exception>
        public async Task<Tuple<string, string>> CompleteText(string sentence)
        {

            RequestContent requestContent = new RequestContent(sentence);


            StringContent textContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/predict";
            var response = await _client.PostAsync(requestUri, textContent);

            try
            {
                response.EnsureSuccessStatusCode();

                var r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return new Tuple<string, string>(responseContent.text, $"{sentence} {responseContent.text}");

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

                throw new SentenceCompletionException(message, ex);
            }

        }

    }
}
