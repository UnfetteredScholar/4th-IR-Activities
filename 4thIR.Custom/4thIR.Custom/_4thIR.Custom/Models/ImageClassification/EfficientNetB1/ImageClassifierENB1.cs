﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IO;
using ImageClassification.Exceptions;

namespace ImageClassification.EfficientNetB1
{
    /// <summary>
    /// Provides functionality for image classification. (Image Classification - EfficientNetB1)
    /// </summary>
    public class ImageClassifierENB1
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

        /// <summary>
        /// Creates a new instance of the ImageClassifierENB1 class
        /// </summary>
        public ImageClassifierENB1(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-classification-efficientnet-b1.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> ClassifyImage(string path)
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

                    string requestUri = "/api/v1/classify";

                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    ResponseContent responseContent = await response.Content.ReadFromJsonAsync< ResponseContent>();

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

                throw new ImageClassificationException(message, ex);
            }
        }
    }
}

