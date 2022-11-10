using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using TextGeneration.Exceptions;

namespace TextGeneration.Gpt2
{
    public class TextGeneratorGpt2
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string generated_text { get; set; }
        }

        private class RequestContent
        {
            public RequestContent(string sentence) => this.sentence = sentence;

            public string sentence { get; set; }

        }

        private static readonly HttpClient _client = new HttpClient();

        public TextGeneratorGpt2()
        {
            _client.BaseAddress = new Uri("https://text-generation-gpt2.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string[]> GenerateText(string sentence)
        {

            RequestContent requestContent = new RequestContent(sentence);


            StringContent textContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/generate";
            var response = await _client.PostAsync(requestUri, textContent);

            try
            {
                response.EnsureSuccessStatusCode();

                var r = await response.Content.ReadAsStringAsync();

                ResponseContent[] responseContents = JsonSerializer.Deserialize<ResponseContent[]>(r);
                string[] generatedTexts = new string[responseContents.Length];

                for (int i = 0; i < responseContents.Length; i++)
                    generatedTexts[i] = responseContents[i].generated_text;

                return generatedTexts;

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

                throw new TextGenerationException(message, ex);
            }

        }

    }
}
