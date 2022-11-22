using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
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

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates a new instance of the ImageClassifierGVB class
        /// </summary>
        public ImageClassifierGVB()
        {
            client.BaseAddress = new Uri("https://image-classification-vit-base-224.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> ClassifyImage(string path)
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

                    ResponseContent[] responseContent = JsonSerializer.Deserialize<ResponseContent[]>(r);

                    return responseContent[0].answer;
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
}
