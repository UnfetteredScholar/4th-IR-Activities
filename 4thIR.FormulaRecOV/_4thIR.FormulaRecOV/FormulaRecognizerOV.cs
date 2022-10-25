using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FormulaRecognition
{
    public enum FormulaType { Normal, HandWritten}
    
    /// <summary>
    /// Provides functionality for detecting formulas in images
    /// </summary>
    public class FormulaRecognizerOV
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string label { get; set; }
            public double score { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Creates an instance of the FormulaRecognizerOV class
        /// </summary>
        public FormulaRecognizerOV()
        {
            
            client.BaseAddress = new Uri("https://image-formula-recognition-openvino.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<Tuple<string, double>> DetectFormula(string path, FormulaType formulaType)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {
                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "file", fileName);

                string requestUri = formulaType == FormulaType.Normal ? "/api/v1/formula" : "/api/v1/polynomial";
                var response = await client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        string label = "Confidence score is low. The formula was not recognized.";
                        return new Tuple<string, double>(label, 0);
                    }
                    else
                    {
                        string r = await response.Content.ReadAsStringAsync();
                        ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                        return new Tuple<string, double>(responseContent.label, responseContent.score);
                    }

                    
                }
                catch (HttpRequestException ex)
                {
                    string message = "";

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        message = "Media type not supported";
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
}
