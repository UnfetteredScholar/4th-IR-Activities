using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HandwrittenTextRecognition.Exceptions;
using System.ComponentModel;
using System.Net.Http.Json;

namespace HandwrittenTextRecognition.OpenVino
{
    public enum Language { [Description("Select A Language")] SelectALanguage, English, Chinese, Japanese }

    /// <summary>
    /// Provides functionality for detecting handwritten English, Chinese and Japanese words in images. (Handwritten Recognition for Engish, Japanese and Chinese Openvino)
    /// </summary>
    public class HandwrittenTextRecognizer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
            public string model { get; set; }
        }

        private HttpClient client = null;


        public HandwrittenTextRecognizer(HttpClient httpClient)
        {
            client = httpClient;
            // client.BaseAddress = new Uri("https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> DetectText(string path, Language language)
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

                    string requestUri = null;
                    switch (language)
                    {
                        case Language.English:
                            requestUri = "https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io/api/v1/recognize_english";
                            break;
                        case Language.Japanese:
                            requestUri = "https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io/api/v1/recognize_japanese";
                            break;
                        case Language.Chinese:
                            requestUri = "https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io/v1/recognize_chinese";
                            break;
                        default:
                            requestUri = "https://image-handwritten-recognition-openvino.ai-sandbox.4th-ir.io/api/v1/recognize_english";
                            break;
                    }
                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent responseContent = await response.Content.ReadFromJsonAsync<ResponseContent>();

                    return responseContent.label;
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

                throw new HandwrittenTextRecognitionException(message, ex);
            }
        }
    }
}
