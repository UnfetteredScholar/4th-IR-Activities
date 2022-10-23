using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace ImageClassification
{
    /// <summary>
    /// Provides functionality for image classification. (Image Classification (MXNET_Resnet50))
    /// </summary>
    public class ImageClassifierMXNET
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates a new instance of the ImageClassifierMXNET class
        /// </summary>
        public ImageClassifierMXNET()
        {
            client.BaseAddress = new Uri("https://image-classification-mxnet-resnet50.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ClassificationLabel[]> ClassifyImage(string path)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf("\\") + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = "/api/v1/classify";

                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ClassificationLabel[] classificationLabels = JsonSerializer.Deserialize<ClassificationLabel[]>(r);

                    return classificationLabels;
                }
                catch (HttpRequestException ex)
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

                    throw new Exception(message, ex);
                }
            }
        }
    }
}
