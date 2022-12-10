using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ObjectDetection.Exceptions;

namespace ObjectDetection.EfficientDet
{
    /// <summary>
    /// Detects objects using Object detection model that can be used for identifying objects in the image, in terms of their location in the image and class they belong to.
    /// </summary>
    public class ObjectDetector
    {
        private class ResponseContent
        {
            public int num_detections { get; set; }
            public double[,] detection_boxes { get; set; }
            public double[] detection_classes { get; set; }
        }

        private HttpClient client = null;

        public ObjectDetector(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-sentiment-analysis-bert-1.ai-sandbox.4th-ir.io");
           // client.DefaultRequestHeaders.Accept.Clear();
           // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Detects objects in image
        /// </summary>
        /// <param name="path">Full or relative path to image file</param>
        /// <returns>Tuple(int NumberOfDetections, double[,] DetectionBoxes, double[] DetectionClasses)</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<int, double[,], double[]>> DetectObject(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            path = @"" + path;

            try
            {
                using (var multipartFormData = new MultipartFormDataContent())
                {
                    StreamContent streamContent = new StreamContent(File.OpenRead(path));
                    streamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    int index = path.LastIndexOf("\\") + 1;
                    string fileName = path.Substring(index);

                    multipartFormData.Add(streamContent, "file", fileName);

                    const string requestUri = "https://image-sentiment-analysis-bert-1.ai-sandbox.4th-ir.io/detection/image";
                    response = await client.PostAsync(requestUri, multipartFormData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    char[] chars = { '[', ']' };
                    r = r.Replace("[[", "[").Replace("]]", "]");
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

                    return new Tuple<int, double[,], double[]>(responseContent.num_detections, responseContent.detection_boxes, responseContent.detection_classes);
                }
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Image must be jpeg or png format";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "ML model not found";
                }
                else
                {
                    message = "Error: Unable to complete object detection";
                }

                throw new ObjectDetectionException(message, ex);
            }

        }
    }



}

