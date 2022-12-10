using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ObjectDetection.Exceptions;

namespace ObjectDetection.Detectron2
{
    /// <summary>
    /// Stores information about detected objects
    /// </summary>
    public class DetectedObject
    {
        /// <summary>
        /// Creates an instance of the DetectedObject class
        /// </summary>
        public DetectedObject()
        {

        }

        public double[][] boxes { get; set; }
        public double[] labels { get; set; }
    }

    /// <summary>
    /// Provides functionality for detecting objects in images
    /// </summary>
    public class ObjectDetectorD2
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public DetectedObject detected_object { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates an instance of the ObjectDetectorFRC class
        /// </summary>
        public ObjectDetectorD2()
        {

            client.BaseAddress = new Uri("https://image-object-dectection-detectron2.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<DetectedObject> DetectObjects(string path)
        {
            path = @"" + path;


            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = "/api/v1/classify";
                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return responseContent.detected_object;
                }
                catch (HttpRequestException ex)
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
