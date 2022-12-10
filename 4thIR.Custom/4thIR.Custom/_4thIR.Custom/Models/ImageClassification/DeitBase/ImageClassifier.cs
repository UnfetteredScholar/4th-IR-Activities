using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using ImageClassification.Exceptions;

namespace ImageClassification.DeitBase
{
    public class ImageClassifier
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string labels { get; set; }
            public string model { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public ImageClassifier()
        {
            _client.BaseAddress = new Uri("https://image-classification-deit-base.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> ClassifyImage(string path)
        {
            path = @"" + path;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                    int index = path.LastIndexOf('\\') + 1;
                    string fileName = path.Substring(index);

                    formData.Add(imageStream, "file", fileName);

                    string requestUri = "/api/v1/classify";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return responseContent.labels;

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

                throw new ImageClassificationException(message, ex);
            }

        }

    }
}
