using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ImageClassification.Exceptions;

namespace ImageClassification.ClassyVision
{
    public class ImageClassifierCV
    {
        private class ResponseContent
        {
            public string label { get; set; }
            public string score { get; set; }
        }

        private HttpClient client = null;

        public ImageClassifierCV(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-classification-classy-vision.ai-sandbox.4th-ir.io");
            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        /// <summary>
        /// Classifies Image file (Image Classification - Classy Vision)
        /// </summary>
        /// <param name="path">Full or relative path to image file- No escape characters required</param>
        /// <returns>String containing the result of classification</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ClassifyImage(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            path = @"" + path;
            try
            {
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    StreamContent fileStreamContent = new StreamContent(File.OpenRead(path));
                    fileStreamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    int index = path.LastIndexOf("\\");
                    index++;

                    string fileName = path.Substring(index);

                    multipartFormContent.Add(fileStreamContent, "file", fileName);

                    string requestUr = "https://image-classification-classy-vision.ai-sandbox.4th-ir.io/api/v1/classify";

                    response = await client.PostAsync(requestUr, multipartFormContent);


                    response.EnsureSuccessStatusCode();

                    string responseContentString = await response.Content.ReadAsStringAsync();
                    char[] chars = new char[] { '[', ']' };
                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(responseContentString.Trim(chars));

                    return responseContent.label;
                }
            }
            catch (Exception ex)
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
                    message = "Unable to classify";
                }

                throw new ImageClassificationException(message, ex);
            }






        }


    }

}
