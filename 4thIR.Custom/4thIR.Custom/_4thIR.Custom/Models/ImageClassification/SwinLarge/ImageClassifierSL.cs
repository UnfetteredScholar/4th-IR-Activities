using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using ImageClassification.Exceptions;


namespace ImageClassification.SwinLarge
{
    public class ImageClassifierSL
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string labels { get; set; }
        }

        private HttpClient _client = null;

        public ImageClassifierSL(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> ClassifyImage(string path)
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

                    string requestUri = "https://image-classification-swin-large.ai-sandbox.4th-ir.io/api/v1/classify";
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

                throw new ImageClassificationException(message, ex);
            }
        }
    }
}
