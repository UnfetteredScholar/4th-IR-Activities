using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ImageClassification.Exceptions;
using System.Net.Http.Json;

namespace ImageClassification.VitBase
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

        private HttpClient client = null;

        /// <summary>
        /// Creates an instance of the ImageClassifierViTBase class
        /// </summary>
        public ImageClassifierViTBase(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-classification-vit-base-384.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Classifies an image
        /// </summary>
        /// <param name="path">Full path to image file</param>
        /// <returns>Classification Label</returns>
        /// <exception cref="Exception"></exception>
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

                    string requestUri = "https://image-classification-vit-base-384.ai-sandbox.4th-ir.io/api/v1/answer";
                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent responseContent = await response.Content.ReadFromJsonAsync<ResponseContent>();

                    return responseContent != null ? responseContent.classes : string.Empty;
                }
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

                throw new ImageClassificationException(message, ex);
            }
        }

    }


}
