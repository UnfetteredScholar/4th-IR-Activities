using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ObjectDetection.Exceptions;
using System.Net.Http.Json;

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

        public double[][] Boxes { get; set; }
        public double[] Labels { get; set; }
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

        private HttpClient client = null;

        /// <summary>
        /// Creates an instance of the ObjectDetectorFRC class
        /// </summary>
        public ObjectDetectorD2(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-object-dectection-detectron2.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<DetectedObject> DetectObjects(string path)
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

                    string requestUri = "https://image-object-dectection-detectron2.ai-sandbox.4th-ir.io/api/v1/classify";
                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent responseContent = await response.Content.ReadFromJsonAsync<ResponseContent>(new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive=true
                    });

                    return responseContent.detected_object;
                }
            }
            catch (Exception ex)
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

                throw new ObjectDetectionException(message, ex);
            }
        }
    }
}
