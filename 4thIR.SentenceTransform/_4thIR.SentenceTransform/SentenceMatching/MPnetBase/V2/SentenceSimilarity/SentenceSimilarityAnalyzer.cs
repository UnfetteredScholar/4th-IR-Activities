using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using SentenceMatching.Exceptions;

namespace SentenceMatching.MPnetBase.V2.SentenceSimilarity
{
    public class MatchedSentence
    {
        public MatchedSentence(string sentence1, string sentence2, double score)
        {
            Sentence1 = sentence1;
            Sentence2 = sentence2;
            Score = score;
        }

        public string Sentence1 { get; set; }
        public string Sentence2 { get; set; }
        public double Score { get; set; }
    }
    public class SentenceSimilarityAnalyzer
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string sentence1 { get; set; }
            public string sentence2 { get; set; }
            public double score { get; set; }
        }


        private static readonly HttpClient _client = new HttpClient();

        public SentenceSimilarityAnalyzer()
        {
            _client.BaseAddress = new Uri("https://text-sentence-transformer-mpnet-base2.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<MatchedSentence[]> MatchSentences(string[] inputSentences)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var requestContent = new { sentences = inputSentences };


                string requestUri = "/api/v1/classify";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));


                response.EnsureSuccessStatusCode();


                string r = await response.Content.ReadAsStringAsync();
                ResponseContent[] responseContent = JsonSerializer.Deserialize<ResponseContent[]>(r);
                MatchedSentence[] matchedSentences = new MatchedSentence[responseContent.Length];

                for (int i = 0; i < responseContent.Length; i++)
                    matchedSentences[i] = new MatchedSentence(responseContent[i].sentence1, responseContent[i].sentence2, responseContent[i].score);

                return matchedSentences;
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

                throw new SentenceMatchingException(message, ex);
            }
        }
    }
}
