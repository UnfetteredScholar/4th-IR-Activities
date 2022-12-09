using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using _4th_IR.Image_Classification.Classification.Client;
using System.Web;
using System.ComponentModel;
using _4th_IR.Image_Classification.Classification.Exceptions;

namespace _4th_IR.Image_Classification.Classification.Client
{
    public class Client
    {

        private class FileUploadResult
        {
            public FileUploadResult() { }
            public string[] scores { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();
        private string _filePath;
        private string _apiUrl;
        private string _input;
        private string _requestUri;

        public Client(string apiUrl, string filePath, string[] input)
        {
            _apiUrl = apiUrl;
            _filePath = @"" + filePath; //"C:\\Users\\Hp\\Desktop\\OIP.jpg";
            _input = string.Join(",", input);
            _requestUri = "/api/v1/classify";
        }
        

        public async Task<string> UploadFile()
        {
           // HttpResponseMessage response = new HttpResponseMessage();
            
            

            if (string.IsNullOrWhiteSpace(_filePath))
            {
                throw new ArgumentNullException(nameof(_filePath));
            }

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File [{_filePath}] not found.");
            }

            string fileExtension = Path.GetExtension(_filePath);
            using (_client)
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), _apiUrl + _requestUri))
                {
                    request.Headers.TryAddWithoutValidation("accept", "application/json");
                    string fileType = "image/";
                    fileType += "jpeg";
                    var multipartContent = new MultipartFormDataContent();

                    var file1 = new ByteArrayContent(File.ReadAllBytes(_filePath));
                    file1.Headers.Add("Content-Type", fileType);
                    multipartContent.Add(file1, "file", Path.GetFileName(_filePath));
                    multipartContent.Add(new StringContent(_input), "classes");
                    request.Content = multipartContent;
                    var response = await _client.SendAsync(request);

                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var res = await response.Content.ReadFromJsonAsync<FileUploadResult>();

                        string x = "It is null";
                        if (res != null)
                        {
                            x = string.Join(" | ", res.scores);
                        }
                        return x;
                    }
                    catch (Exception ex)
                    {
                        string message = "";

                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            message = "Invalid media use png/jpg";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            message = "ML model not found";
                        }
                        else
                        {
                            message = "Error: Unable to complete operation";
                        }

                        throw new classificationException(message, ex);
                    }



                }
            }
        }
    }

    
}
