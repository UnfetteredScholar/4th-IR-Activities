using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using PoseEstimation.Exceptions;

namespace PoseEstimation.OpenVino
{
    public class PoseEstimator
    {
        private static readonly HttpClient _client = new HttpClient();

        public PoseEstimator()
        {
            _client.BaseAddress = new Uri("https://image-single-human-pose-estimation-openvino.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task DetectFigures(string path, double threshold, string outputPath = "")
        {
            path = @"" + path;
            outputPath = @"" + outputPath;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    int index = path.LastIndexOf('\\') + 1;
                    string fileName = path.Substring(index);

                    formData.Add(imageStream, "file", fileName);

                    StringContent stringContent = new StringContent(threshold.ToString(), Encoding.UTF8, "text/plain");
                    formData.Add(stringContent, "threshold");

                    string requestUri = "/api/v1/estimate/";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    var outImage = await response.Content.ReadAsByteArrayAsync();

                    string outFileName;
                    if (Directory.Exists(outputPath))
                    {
                        outFileName = outputPath + "\\" + "DF_" + fileName;
                    }
                    else
                    {
                        outFileName = "DF_" + fileName;
                    }

                    File.WriteAllBytes(outFileName, outImage);
                }

            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Media type not supported";
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
