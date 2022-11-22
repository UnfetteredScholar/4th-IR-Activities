using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using PoseEstimation.Exceptions;

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

        private static readonly HttpClient _client = new HttpClient();

        public PoseEstimator()
        {
            _client.BaseAddress = new Uri("https://image-pose-estimation-mmpose.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DetectedFigure[]> DetectFigures(string path)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = "/api/v1/estimate";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    ResponseContent[] responseContents = JsonSerializer.Deserialize<ResponseContent[]>(r);

                    DetectedFigure[] detectedFigures = new DetectedFigure[responseContents.Length];

                    for (int i = 0; i < responseContents.Length; i++)
                        detectedFigures[i] = new DetectedFigure(responseContents[i].keypoints, responseContents[i].score, responseContents[i].area);

                    return detectedFigures;
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
}
