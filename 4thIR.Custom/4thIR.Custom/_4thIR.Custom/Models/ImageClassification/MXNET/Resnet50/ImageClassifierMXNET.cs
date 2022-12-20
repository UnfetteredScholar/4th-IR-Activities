using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using ImageClassification.Exceptions;
using System.Net.Http.Json;

namespace ImageClassification.MXNET.Resnet50
{
    /// <summary>
    /// Provides functionality for image classification. (Image Classification (MXNET_Resnet50))
    /// </summary>
    public class ImageClassifierMXNET
    {
        private HttpClient client = null;

        /// <summary>
        /// Creates a new instance of the ImageClassifierMXNET class
        /// </summary>
        public ImageClassifierMXNET(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-classification-mxnet-resnet50.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ClassificationLabel[]> ClassifyImage(string path)
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

                    string requestUri = "https://image-classification-mxnet-resnet50.ai-sandbox.4th-ir.io/api/v1/classify";

                    response = await client.PostAsync(requestUri, formData);
                    response.EnsureSuccessStatusCode();

                    ClassificationLabel[] classificationLabels = await response.Content.ReadFromJsonAsync<ClassificationLabel[]>(new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return classificationLabels;
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Image not jpg/png";
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
