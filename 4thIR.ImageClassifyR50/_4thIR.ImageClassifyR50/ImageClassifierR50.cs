using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;

namespace ImageClassificationR50
{
    public class ImageClassifierR50
    {
        private static readonly HttpClient client=new HttpClient();

        public ImageClassifierR50()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> ClassifyImage(string path)
        {
            path = @"" + path;

            using(var multipartContent=new MultipartFormDataContent())
            {
                byte[] bytes = File.ReadAllBytes(path);
                string imageBase64 = Convert.ToBase64String(bytes);

                RequestContent request = new RequestContent();
                request.base64_image_string = imageBase64;

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"), "base64_image_string");

                StreamContent streamContent=new StreamContent(File.OpenRead(path));
                streamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf("\\");
                index++;

                string fileName = path.Substring(index);

                multipartContent.Add(streamContent,"file",fileName);

               

                string requestUri = "https://image-classification-resnet50-1.ai-sandbox.4th-ir.io/api/v1/classify";
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
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception("string too long", ex);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        throw new Exception("ML model not found", ex);
                    }
                    else
                    {
                        throw new Exception("Error: Unable to classify image", ex);
                    }
                }
            }
        }
    }
}
