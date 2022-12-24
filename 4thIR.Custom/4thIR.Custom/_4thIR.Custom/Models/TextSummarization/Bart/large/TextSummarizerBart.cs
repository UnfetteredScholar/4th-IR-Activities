using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using TextSummarization.Exceptions;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TextSummarization.Bart.large
{
    public class TextSummarizerBart
    {
        private string _userSentence;
        private string _appType;

        public string UserSentence { get => _userSentence; set => _userSentence = value; }
        public HttpClient myClient { get; set; }
        public string ApiUrl { get; set; }
        public string AppType { get => _appType; set => _appType = value; }

        public TextSummarizerBart(string input, HttpClient client)
        {
            UserSentence = input;
            myClient = client;
            ApiUrl = "https://text-summarization-bart-large-cnn.ai-sandbox.4th-ir.io/api/v1/classify";
            AppType = "application/json";
        }

        public class Root
        {
            public List<Text> texts { get; set; }
        }

        public class Text
        {
            public string summary_text { get; set; }
        }


        // API Call
        public async Task<string> summarize()
        {
            var requestObj = new { sentence = UserSentence };


                using (var request = new HttpRequestMessage(new HttpMethod("POST"), ApiUrl))
                {
                    request.Headers.TryAddWithoutValidation("accept", AppType);

                    request.Content = new StringContent(JsonSerializer.Serialize(requestObj));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(AppType);

                    var response = await myClient.SendAsync(request);

                    try
                    {
                        response.EnsureSuccessStatusCode();
                        string results = await response.Content.ReadAsStringAsync();

                        var content = JsonSerializer.Deserialize<Root>(results);
                        string res = content.texts.ElementAt(0).summary_text;


                        return res;
                    }
                    catch (Exception ex)
                    {
                        string message = "";

                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            message = "String is too long";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            message = "ML model not found";
                        }
                        else
                        {
                            message = "Error: Unable to complete operation  || " + ex.Message;
                        }
                        throw new TextSummarizationException(message, ex);

                    }
                }
            
        }
    }
}
