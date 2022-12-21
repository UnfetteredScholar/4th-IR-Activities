using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ObjectDetection.Exceptions;

namespace ObjectDetection.Yolov4
{
    public class ObjectDetectorY4
    {
        private HttpClient _client = null;

        public ObjectDetectorY4(HttpClient client)
        {
            _client = client;
            //_client.BaseAddress = new Uri("https://image-object-detection-yolov4.ai-sandbox.4th-ir.io");
            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task DetectObject(string path, string storageLocation = "")
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

                    string requestUri = "https://image-object-detection-yolov4.ai-sandbox.4th-ir.io/api/v1/detection";
        
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsByteArrayAsync();
                    string name;
                    if (Directory.Exists(storageLocation))
                        name = storageLocation + "\\DetectedObject_" + fileName;
                    else
                        name = "DetectedObject_"+fileName;


                    File.WriteAllBytes(name, responseContent);
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
