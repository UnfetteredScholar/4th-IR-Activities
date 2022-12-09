using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SentenceMatching.Exceptions;

namespace SentenceMatching.SentenceSimilarity.Minilm
{
    public class SimilarityScore
    {
        public SimilarityScore()
        {

        }

        public string Sentece1 { get; set; }
        public string Sentece2 { get; set; }
        public double Score { get; set; }
    }
    public class TextSimilarityComparer
    {


        private HttpClient _client = null;

        public TextSimilarityComparer(HttpClient client)
        {
            _client = client;
        }

        public async Task<SimilarityScore[]> CompareSentences(string[] sentenceGroup1, string[] sentenceGroup2)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { sentences1 = sentenceGroup1, sentences2 = sentenceGroup2 };

                string requestUri = "https://text-paraphrase-minilm-v2.ai-sandbox.4th-ir.io/api/v1/classify";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                var jsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                SimilarityScore[] responseContents = await response.Content.ReadFromJsonAsync<SimilarityScore[]>(jsonSerializerOptions);


                return responseContents;
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "String too long";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    message = "ML model not found";
                }
                else
                {
                    message = "Error: Unable to complete operation";
                }

                throw new SentenceMatchingException(message, ex);
            }
        }
    }
}
