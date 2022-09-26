using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace EfficientDetObjectDetection
{
    /// <summary>
    /// Detects objects using Object detection model that can be used for identifying objects in the image, in terms of their location in the image and class they belong to.
    /// </summary>
    public class ObjectDetector
    {
        private static readonly HttpClient client=new HttpClient();

        public ObjectDetector()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Detects objects in image
        /// </summary>
        /// <param name="path">Full or relative path to image file</param>
        /// <returns>Tuple(int NumberOfDetections, double[,] DetectionBoxes, double[] DetectionClasses)</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<int, double[,], double[]>> DetectObject(string path)
        {
            path = "" + path;

            using(var multipartFormData=new MultipartFormDataContent())
            {
                StreamContent streamContent=new StreamContent(File.OpenRead(path));
                streamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf("\\") + 1;
                string fileName = path.Substring(index);

                multipartFormData.Add(streamContent, "file", fileName);

                const string requestUri = "https://image-sentiment-analysis-bert-1.ai-sandbox.4th-ir.io/detection/image";
                var response = await client.PostAsync(requestUri, multipartFormData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r=await response.Content.ReadAsStringAsync();
                    char[] chars = { '[', ']' };
                    r = r.Replace("[[", "[").Replace("]]", "]");
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

                    return new Tuple<int, double[,], double[]>(responseContent.num_detections, responseContent.detection_boxes, responseContent.detection_classes);
                }
                catch(HttpRequestException ex)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception("Image must be jpeg or png format", ex);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        throw new Exception("ML model not found", ex);
                    }
                    else
                    {
                        throw new Exception("Error: Unable to complete object detection", ex);
                    }
                }

            }
        }



    }
}
