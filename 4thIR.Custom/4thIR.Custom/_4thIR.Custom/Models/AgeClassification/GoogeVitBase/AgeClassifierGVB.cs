using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using AgeClassification.Exceptions;

namespace AgeClassification.GoogeVitBase
{
    public class AgeClassifierGVB
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string age { get; set; }
            public double probability { get; set; }

        }

        private HttpClient _client = null;

        public AgeClassifierGVB(HttpClient httpClient)
        {
            _client = httpClient;
            // _client.BaseAddress = new Uri("https://image-vit-age-classifier.ai-sandbox.4th-ir.io");
            // _client.DefaultRequestHeaders.Accept.Clear();
            // _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AgeProbabilityPair[]> ClassifyAge(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");
                    
                    string fileName = Path.GetFileName(path);

                    formData.Add(imageStream, "file", fileName);

                    string requestUri = "https://image-vit-age-classifier.ai-sandbox.4th-ir.io/api/v1/classify";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ResponseContent[] responseContents = JsonSerializer.Deserialize<ResponseContent[]>(r);

                    AgeProbabilityPair[] ageProbabilityPairs = new AgeProbabilityPair[responseContents.Length];

                    for (int i = 0; i < responseContents.Length; i++)
                        ageProbabilityPairs[i] = new AgeProbabilityPair(responseContents[i].age, responseContents[i].probability);

                    return ageProbabilityPairs;
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Invalid image format";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "ML model not found";
                }
                else
                {
                    message = "Error: Unable to complete operation";
                }

                throw new AgeClassificationException(message, ex);
            }

        }
    }
}
