using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NameEntityRecognition.Exceptions;

namespace NameEntityRecognition.BERT
{
    public class NameEntityRecognizerBERT
    {

        private static readonly HttpClient client = new HttpClient();

        public NameEntityRecognizerBERT()
        {
            client.BaseAddress = new Uri("https://text-name-entity-recognition-bert-1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TextValuePair[]> RecognizeNameEntity(string sentence)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    StringContent stringContent = new StringContent(sentence, Encoding.UTF8, "text/plain");

                    formData.Add(stringContent, "sentence");

                    string requestUri = "/api/v1/sentence";
                    response = await client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    var jsonOptions = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    TextValuePair[] result = await response.Content.ReadFromJsonAsync<TextValuePair[]>(jsonOptions);

                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Invalid string error";
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



        public async Task<TextValuePair[]> RecognizeNameEntityInFile(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent streamContent = new StreamContent(File.OpenRead(path));

                    int index = path.LastIndexOf("\\") + 1;
                    string fileName = path.Substring(index);

                    formData.Add(streamContent, "text_file", fileName);

                    string requestUri = "/api/v1/sentence";
                    response = await client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    TextValuePair[] result = JsonConvert.DeserializeObject<TextValuePair[]>(r);

                    return result;
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Invalid string error";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "ML model not found";
                }
                else
                {
                    message = "Error: Unable to complete operation";
                }

                throw new NameEntityRecognitionException(message, ex);
            }
        }
    }
}

