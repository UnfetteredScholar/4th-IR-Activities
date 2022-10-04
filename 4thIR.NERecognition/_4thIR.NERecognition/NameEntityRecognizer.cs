﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace NameEntityRecognition
{
    

    public class NameEntityRecognizer
    {
        private class RequestContent
        {
            public RequestContent(string sentence)
            {
                this.sentence = sentence;
            }

            public string sentence { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        public NameEntityRecognizer(double timeout=60)
        {
            client.BaseAddress = new Uri("https://text-name-entity-recognition-flair.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(timeout);
        }

        public async Task<WordTag[]> RecognizeNameEntity(string sentence)
        {
            RequestContent requestContent = new RequestContent(sentence);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

            string requestUri = "/predict/sentence";
            var response = await client.PostAsync(requestUri, stringContent);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                WordTag[] result = JsonConvert.DeserializeObject<WordTag[]>(r);

                return result;
            }
            catch (HttpRequestException ex)
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
                    message = "Error: Unable to complete operation";
                }

                throw new Exception(message, ex);
            }
        }
    }
}
