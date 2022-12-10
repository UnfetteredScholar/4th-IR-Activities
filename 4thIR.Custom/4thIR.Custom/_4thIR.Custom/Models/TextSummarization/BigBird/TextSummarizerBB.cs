using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TextSummarization.Exceptions;

namespace TextSummarization.BigBird
{
    public class TextSummarizerBB
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string description { get; set; }
            public string model { get; set; }
        }

        private class RequestContent
        {
            public RequestContent(string articleText)
            {
                article = articleText;
            }

            public string article { get; set; }
        }
        private HttpClient client = null;

        public TextSummarizerBB(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-summarization-google-bigbird-1.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleText"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<string, string>> SummarizeText(string articleText)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    RequestContent requestContent = new RequestContent(articleText);
                    formData.Add(new StringContent(articleText, Encoding.UTF8, "text/plain"), "article");

                    string requestUri = "https://text-summarization-google-bigbird-1.ai-sandbox.4th-ir.io/summarize/article";
                    response = await client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();

                    char[] chars = { '[', ']' };

                    ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(chars));

                    return new Tuple<string, string>(responseContent.description, responseContent.model);
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
                    message = "Error: Unable to complete operation";
                }

                throw new TextSummarizationException(message, ex);
            }
        }



        public async Task<Tuple<string, string>> SummarizeTextFile(string path)
        {
            path = @"" + path;

            string sentence = File.ReadAllText(path);
            var res = await SummarizeText(sentence);

            return res;
        }

    }
}
