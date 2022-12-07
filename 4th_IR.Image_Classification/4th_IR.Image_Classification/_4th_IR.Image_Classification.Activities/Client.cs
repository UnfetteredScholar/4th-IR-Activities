using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace _4th_IR.Image_Classification.Activities
{
    public class Client
    {
        private HttpClient _client;
        private string _filePath;
        private string _apiUrl;
        private string _input;

        public Client(string apiUrl, string filePath, string[] input)
        {
            _client = new HttpClient();
            _apiUrl = "https://image-classification-clip-vit.ai-sandbox.4th-ir.io/api/v1/classify";
            _filePath = "C:\\Users\\Hp\\Desktop\\OIP.jpg";
            _input = String.Join(",", input);
        }

        public async Task<String> UploadFile()
        {

            if (string.IsNullOrWhiteSpace(_filePath))
            {
                throw new ArgumentNullException(nameof(_filePath));
            }

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File [{_filePath}] not found.");
            }
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), _apiUrl))
                    {
                        request.Headers.TryAddWithoutValidation("accept", "application/json");

                        var multipartContent = new MultipartFormDataContent();
                        var file1 = new ByteArrayContent(File.ReadAllBytes(_filePath));
                        file1.Headers.Add("Content-Type", "image/jpeg");
                        multipartContent.Add(file1, "file", Path.GetFileName("OIP.jpg"));
                        multipartContent.Add(new StringContent(_input), "classes");
                        request.Content = multipartContent;

                        var response = await httpClient.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        var res = await response.Content.ReadFromJsonAsync<FileUploadResult>();

                        string x = "It is null";
                        if (res != null)
                        {
                            x = String.Join(" | ", res.scores);
                        }
                        return x; ;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }

    public class FileUploadResult
    {
        public string[] scores { get; set; }
    }
}
