using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using PoseEstimation.Exceptions;
using System.Net.Http.Json;

namespace PoseEstimation.mmPose
{


    /// <summary>
    /// Provides functionality for detecting human figures images
    /// </summary>
    public class PoseEstimator
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public double[][] keypoints { get; set; }
            public double score { get; set; }
            public double area { get; set; }
        }

        private HttpClient _client = null;

        public PoseEstimator(HttpClient client)
        {
            _client = client;
            //_client.BaseAddress = new Uri("https://image-pose-estimation-mmpose.ai-sandbox.4th-ir.io");
            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DetectedFigure[]> DetectFigures(string path)
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

                    string requestUri = "https://image-pose-estimation-mmpose.ai-sandbox.4th-ir.io/api/v1/estimate";
                    response = await _client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    DetectedFigure[] detectedFigures = await response.Content.ReadFromJsonAsync<DetectedFigure[]>(new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return detectedFigures;
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

                throw new PoseEstimationException(message, ex);
            }

        }
    }
}
