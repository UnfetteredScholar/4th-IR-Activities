using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using SemanticSegmentation.Exceptions;

namespace SemanticSegmentation.PSPNet
{
    public class SemanticSegmentizerPSPNet
    {
        private static readonly HttpClient _client = new HttpClient();

        public SemanticSegmentizerPSPNet()
        {
            _client.BaseAddress = new Uri("https://image-semantic-segmentation-pspnet.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task SegmentImage(string path, string storageLocation = "")
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = "/api/v1/segment";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsByteArrayAsync();
                    string name;
                    if (Directory.Exists(storageLocation))
                        name = storageLocation + "\\SegmentedImage_" + fileName;
                    else
                        name = fileName;


                    File.WriteAllBytes(name, responseContent);
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

                    throw new SemanticSegmentationException(message, ex);
                }

            }
        }

    }
}
