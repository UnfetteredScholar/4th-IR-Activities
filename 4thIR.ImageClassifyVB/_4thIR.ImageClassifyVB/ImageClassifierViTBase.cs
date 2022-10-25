using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ImageClassification
{
    /// <summary>
    /// Provides functionality for image classification.(Image Classification - ViT-Base)
    /// </summary>
    public class ImageClassifierViTBase
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string classes { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates an instance of the ImageClassifierViTBase class
        /// </summary>
        public ImageClassifierViTBase()
        {
            client.BaseAddress = new Uri("https://image-classification-vit-base-384.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Classifies an image
        /// </summary>
        /// <param name="path">Full path to image file</param>
        /// <returns>Classification Label</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ClassifyImage(string path)
        {
            path = @"" + path;

            
            
            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = "/api/v1/answer";
                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return responseContent != null ? responseContent.classes : String.Empty;
                }
                catch (HttpRequestException ex)
                {
                    string message = "";

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        message = "Media type not supported";
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
