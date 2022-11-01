using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using _4thIR.TextSpeechGen.TextToSpeech.Exceptions;

namespace _4thIR.TextSpeechGen.TextToSpeech.Openvino
{
    public class TextToSpeechGenerator
    {


        private static readonly HttpClient _client = new HttpClient();

        public TextToSpeechGenerator()
        {
            _client.BaseAddress = new Uri("https://text-to-speech-openvino-en0001.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task GenerateAudio(string text, string outputPath = "")
        {
            var requestContent = new { sentence = text };

            StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/api/v1/generate";
            var response = await _client.PostAsync(requestUri, stringContent);

            try
            {
                var responseContent = await response.Content.ReadAsByteArrayAsync();

                string fileName = Directory.Exists(outputPath) ? outputPath + "\\Audio.wav" : "Audio.wav";

                File.WriteAllBytes(fileName, responseContent);

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

                throw new TextToSpeechException(message, ex);
            }
        }
    }
}
