using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ObjectDetectionFRC
{
    

    /// <summary>
    /// Provides functionality for detecting objects in images
    /// </summary>
    public class ObjectDetectorFRC
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public DetectedObject detected_objects { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();
         
        /// <summary>
        /// Creates an instance of the ObjectDetectorFRC class
        /// </summary>
        public ObjectDetectorFRC()
        {
            client.BaseAddress = new Uri("https://image-detection-fasterrcnn-resnet50fpn.ai-sandbox.4th-ir.io");
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

                    r = r.Remove(r.IndexOf('['),1);
                    r=r.Remove(r.LastIndexOf(']'),1);
                    
                    r = '{'+ r.Replace("{", "").Replace("}", "") + '}';
                    r=r.Remove(1,r.IndexOf(':'));

                    DetectedObject responseContent = JsonSerializer.Deserialize<DetectedObject>(r);
                    
                    return responseContent;
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
