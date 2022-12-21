using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using FormulaRecognition.Exceptions;
using System.ComponentModel;
using System.Net.Http.Json;

namespace FormulaRecognition.OpenVino
{
    public enum FormulaType { [Description("Select An Item")] SelectAnItem, [Description("Normal")] Normal, [Description("Handwritten")] HandWritten }


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

        private HttpClient client = new HttpClient();

        /// <summary>
        /// Creates an instance of the FormulaRecognizerOV class
        /// </summary>
        public FormulaRecognizerOV(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://image-formula-recognition-openvino.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<Tuple<string, double>> DetectFormula(string path, FormulaType formulaType)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent imageStream = new StreamContent(File.OpenRead(path));
                    imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/png");

                    string fileName = Path.GetFileName(path);

                    formData.Add(imageStream, "file", fileName);

                    string requestUri = formulaType == FormulaType.Normal ? "https://image-formula-recognition-openvino.ai-sandbox.4th-ir.io/api/v1/formula" : "https://image-formula-recognition-openvino.ai-sandbox.4th-ir.io/api/v1/polynomial";
                    response = await client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        string label = "Confidence score is low. The formula was not recognized.";
                        return new Tuple<string, double>(label, 0);
                    }
                    else
                    {
                        ResponseContent responseContent = await response.Content.ReadFromJsonAsync<ResponseContent>();

                        return new Tuple<string, double>(responseContent.label, responseContent.score);
                    }
                }
            }
            catch (Exception ex)
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

                throw new FormulaRecognitionException(message, ex);
            }
        }
    }
}
