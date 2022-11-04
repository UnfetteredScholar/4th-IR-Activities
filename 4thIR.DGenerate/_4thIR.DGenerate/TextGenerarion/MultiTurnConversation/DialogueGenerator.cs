using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using _4thIR.DGenerate.TextGenerarion.Exceptions;

namespace _4thIR.DGenerate.TextGenerarion.MultiTurnConversation
{
    public class DialogueGenerator
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string bot { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public DialogueGenerator()
        {
            _client.BaseAddress = new Uri("https://text-generation-dialogpt.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> GenerateDialogueResponse(string text)
        {
            var builder = new UriBuilder("https://text-generation-dialogpt.ai-sandbox.4th-ir.io/api/v1/generate");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["text"] = text;
            builder.Query = query.ToString();


            var response = await _client.PostAsync(builder.ToString(), null);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return responseContent.bot;
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
                    message = "Error: Unable to complete operation";
                }

                throw new DialogueGenerationException(message, ex);
            }
        }

    }
}
