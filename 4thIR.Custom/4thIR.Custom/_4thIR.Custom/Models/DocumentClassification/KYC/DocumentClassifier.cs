using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;
using System.ComponentModel;
using DocumentClassification.Exceptions;
using System.Net.Http.Json;

namespace DocumentClassification.KYC
{
    /// <summary>
    /// Document Type
    /// </summary>
    public enum DocumentType
    {
        [Description("Select Item")]
        selectItem,
        [Description("PDF")]
        pdf,
        [Description("Image")]
        image
    }

    /// <summary>
    /// Document Class
    /// </summary>
    public enum DocumentClass { passport, commercial_register, other }

    /// <summary>
    /// Provides a class for performing document classification.
    /// </summary>
    public class DocumentClassifier
    {
        private class ResponseContent
        {
            public string document_class { get; set; }
        }

        private HttpClient client = null;

        /// <summary>
        /// Initializes a new instance of the DocumentClassifier class
        /// </summary>
        public DocumentClassifier(HttpClient httpClient)
        {
            client = httpClient;
            //client.BaseAddress = new Uri("https://text-document-classifier-4thir-kyc-custom-1.ai-sandbox.4th-ir.io");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Converts file to Base64 encoded string
        /// </summary>
        /// <param name="path">Full or relative path to file</param>
        /// <returns></returns>
        public string ReadDocumentContentBase64(string path)
        {
            path = @"" + path;

            byte[] bytes = File.ReadAllBytes(path);
            string file = Convert.ToBase64String(bytes);

            return file;
        }

        /// <summary>
        /// Classifies documents based on their content
        /// </summary>
        /// <param name="documentContent">Base64 encoded string of file</param>
        /// <param name="documentType">File type</param>
        /// <returns>Document class {passport, commercial register, other}</returns>
        public async Task<string> ClassifyDocument(string path, DocumentType documentType)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                string requestUrl = "https://text-document-classifier-4thir-kyc-custom-1.ai-sandbox.4th-ir.io/api/v1/classify/";

                var request = new { document_content = ReadDocumentContentBase64(path), document_type = documentType.ToString() };

                var requestJson = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                response = await client.PostAsync(requestUrl, requestJson);

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                return responseContent?.document_class != null ? responseContent.document_class : " ";
            }
            catch (Exception ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Error processing document_content";
                }
                else
                {
                    message = "Error";
                }

                throw new DocumentClassificationException(message, ex);
            }



        }

    }
}
