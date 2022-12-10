using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PartOfScpeech.Exceptions;

namespace PartOfScpeech.Tagging.Flair
{
    public class PartOfSpeechIdentifier
    {
        private class RequestContent
        {
            public RequestContent(string sentence)
            {
                this.sentence = sentence;
            }

            public string sentence { get; set; }
        }

        private HttpClient client = null;

        public PartOfSpeechIdentifier(HttpClient httpClient)
        {
            client = httpClient;

            //client.BaseAddress = new Uri("https://text-part-of-speech-flair.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WordTag[]> IdentifyPartOfSpeech(string sentence)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                RequestContent requestContent = new RequestContent(sentence);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

                string requestUri = "https://text-part-of-speech-flair.ai-sandbox.4th-ir.io/tag/sentence";
                response = await client.PostAsync(requestUri, stringContent);


                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                WordTag[] result = JsonConvert.DeserializeObject<WordTag[]>(r);

                return result;
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

                throw new PartOfSpeechIdentificationException(message, ex);
            }
        }
    }
}
