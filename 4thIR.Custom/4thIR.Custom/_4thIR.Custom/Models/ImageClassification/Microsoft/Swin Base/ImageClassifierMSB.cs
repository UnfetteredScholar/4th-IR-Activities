using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ImageClassification.Exceptions;

namespace ImageClassification.Microsoft.SwinBase
{
    public class ImageClassifierMSB
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
        }

        private HttpClient _client = null;

        public ImageClassifierMSB(HttpClient client)
        {
            _client = client;

        }

        public async Task<string> ClassifyImage(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            path = @"" + path;

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    string fileName = Path.GetFileName(path);
                    formData.Add(imageStream, "file", fileName);

                    string requestUri = "https://image-classification-microsoft-swin-base.ai-sandbox.4th-ir.io/api/v1/classify";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent[] responseContent = await response.Content.ReadFromJsonAsync<ResponseContent[]>();

                    return responseContent[0].answer;

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
