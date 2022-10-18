using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ImageClassificationCV
{
    public class ImageClassifierCV
    {
        private class ResponseContent
        {
            public string label { get; set; }
            public string score { get; set; }
        }

        private static readonly HttpClient client=new HttpClient();

        public ImageClassifierCV()
        {
            client.BaseAddress = new Uri("https://image-classification-classy-vision.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        /// <summary>
        /// Classifies Image file (Image Classification - Classy Vision)
        /// </summary>
        /// <param name="path">Full or relative path to image file- No escape characters required</param>
        /// <returns>String containing the result of classification</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ClassifyImage(string path)
        {
            path = @"" + path;

            using(var multipartFormContent= new MultipartFormDataContent())
            {
                StreamContent fileStreamContent=new StreamContent(File.OpenRead(path));
                fileStreamContent.Headers.ContentType=new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf("\\");
                index++;

                string fileName = path.Substring(index);

                multipartFormContent.Add(fileStreamContent,"file", fileName);

                string requestUr = "/api/v1/classify";

                var response = await client.PostAsync(requestUr, multipartFormContent);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string responseContentString=await response.Content.ReadAsStringAsync();
                    char[] chars= new char[] { '[', ']' };
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(responseContentString.Trim(chars));

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
                        throw new Exception("Unable to classify", ex);
                    }
                }






            }


        }

    }
}
