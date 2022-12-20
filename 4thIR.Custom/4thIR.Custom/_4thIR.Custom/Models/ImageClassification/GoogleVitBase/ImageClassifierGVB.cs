using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ImageClassification.Exceptions;

namespace ImageClassification.GoogleVitBase
{
    /// <summary>
    /// Provides functionality for image classification. (Image Classification - Google Vit Base)
    /// </summary>
    public class ImageClassifierGVB
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
        }

        private HttpClient client = null;

        /// <summary>
        /// Creates a new instance of the ImageClassifierGVB class
        /// </summary>
        public ImageClassifierGVB(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-classification-vit-base-224.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

                    string fileName = Path.GetFileName(path);

                    formData.Add(imageStream, "file", fileName);

                    string requestUri = "https://image-classification-vit-base-224.ai-sandbox.4th-ir.io/api/v1/classify";

                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent[] responseContent = await response.Content.ReadFromJsonAsync< ResponseContent[]>();

                    return responseContent[0].answer;
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "String too long";
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

