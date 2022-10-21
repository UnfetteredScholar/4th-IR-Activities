using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace ImageClassification
{
    /// <summary>
    /// Provides functionality for image classification. (Image Classification - EfficientNetB1)
    /// </summary>
    public class ImageClassifierENB1
    {
        
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
            public string model { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates a new instance of the ImageClassifierENB1 class
        /// </summary>
        public ImageClassifierENB1()
        {
            client.BaseAddress = new Uri("https://image-classification-efficientnet-b1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Tuple<string,string>> ClassifyImage(string path)
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

                    string r=await response.Content.ReadAsStringAsync();

                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return new Tuple<string, string>(responseContent.label,responseContent.model);
                }
                catch(HttpRequestException ex)
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

                    throw new Exception(message, ex);
                }
            }
        }
    }
}
