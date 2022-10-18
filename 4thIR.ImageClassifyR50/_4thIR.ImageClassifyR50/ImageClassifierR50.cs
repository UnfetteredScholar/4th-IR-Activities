using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;

namespace ImageClassificationR50
{
    public class ImageClassifierR50
    {
        private class ResponseContent
        {
            public string label { get; set; }
            public string model { get; set; }
        }

        private static readonly HttpClient client=new HttpClient();

        public ImageClassifierR50()
        {
            client.BaseAddress = new Uri("https://image-classification-resnet50-1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> ClassifyImage(string path)
        {
            path = @"" + path;

            using(var multipartContent=new MultipartFormDataContent())
            {
                /*
                byte[] bytes = File.ReadAllBytes(path);
                string imageBase64 = Convert.ToBase64String(bytes);

                RequestContent request = new RequestContent();
                request.base64_image_string = imageBase64;

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"), "base64_image_string");
                */

                StreamContent streamContent=new StreamContent(File.OpenRead(path));
                streamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf("\\")+1;


                string fileName = path.Substring(index);

                multipartContent.Add(streamContent,"file",fileName);

               

                string requestUri = "/api/v1/classify";
                var response = await client.PostAsync(requestUri, multipartContent);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    char[] chars = new char[] { '[', ']' };
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(chars));

                    return responseContent.label;
                }
                catch(HttpRequestException ex)
                {
                    string message = "";

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        message = "string too long";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        message = "ML model not found";
                    }
                    else
                    {
                        message = "Error: Unable to classify image";
                    }

                    throw new Exception(message, ex);
                }
            }
        }
    }
}
