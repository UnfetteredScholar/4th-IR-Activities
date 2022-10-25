using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WordRecognition
{
    public enum Language { English, Chinese, Japanese}

    // <summary>
    /// Provides functionality for detecting handwritten English, Chinese and Japanese words in images. (Handwritten Recognition for Engish, Japanese and Chinese Openvino)
    /// </summary>
    public class HandwrittenRecognizer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
            public string model { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates an instance of the HandwrittenRecognizer class
        /// </summary>
        /// <param name="httpClientFactory">HttpClientFactory object for creating HttpClient instances</param>
        public HandwrittenRecognizer()
        {
            client.BaseAddress = new Uri("https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> DetectWords(string path,Language language)
        {
            path = @"" + path;

            
            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);
                
                string requestUri = null;
                switch (language)
                {
                    case Language.English:
                        requestUri = "/api/v1/recognize_english";
                        break;
                    case Language.Japanese:
                        requestUri = "/api/v1/recognize_japanese";
                        break;
                    case Language.Chinese:
                        requestUri = "/v1/recognize_chinese";
                        break;
                    default:
                        requestUri = "/api/v1/recognize_english";
                        break;
                }

                
                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return responseContent.label;
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
