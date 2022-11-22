using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using ImageClassification.Exceptions;

namespace ImageClassification.Vissl
{
    /// <summary>
    /// Class containing functionality for image classification
    /// </summary>
    public class ImageClassifier
    {
        private class ResponseContent
        {
            public string label { get; set; }
            public string model { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes ImageClassifier object
        /// </summary>
        public ImageClassifier()
        {
            client.BaseAddress = new Uri("https://image-classification-vissl.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }


        /// <summary>
        /// Performs classification of an image
        /// </summary>
        /// <param name="path">Full or relative file path to image resource png/jpg</param>
        /// <returns>Image Label</returns>
        public async Task<string> ClassifyImage(string path)
        {
            string requestUri = "/api/v1/classify";
            path = @"" + path;

            using (var multipartFormContent = new MultipartFormDataContent())
            {
                StreamContent fileStreamContent = new StreamContent(File.OpenRead(path));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                int index = path.LastIndexOf('\\');
                index = index == -1 ? 0 : index + 1;

                string fileName = path.Substring(index);
                multipartFormContent.Add(fileStreamContent, "file", fileName);


                var response = await client.PostAsync(requestUri, multipartFormContent);

                try
                {
                    response.EnsureSuccessStatusCode();

                    var r = await response.Content.ReadAsStringAsync();
                    //char[] param = new char[] { '[', ']' };
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

                    return responseContent?.label != null ? responseContent.label : string.Empty;
                }
                catch (Exception ex)
                {
                    string message = "";

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        message = "string too long";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        message = "ML model not found";
                    }
                    else
                    {
                        message = "Error: Operation failed";
                    }

                    throw new ImageClassificationException(message, ex);
                }

            }


        }
    }
}
