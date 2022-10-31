using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using _4thIR.DocClassify.DocumentClassification.Exceptions;

namespace _4thIR.DocClassify.DocumentClassification.KYC
{
    /// <summary>
    /// Document Type
    /// </summary>
    public enum DocumentType { pdf, image }

    /// <summary>
    /// Document Class
    /// </summary>
    public enum DocumentClass { passport, commercial_register, other }

    /// <summary>
    /// Provides a class for performing document classification.
    /// </summary>
    public class DocumentClassifier
    {
        private class RequestContent
        {
            public string document_content { get; set; }
            public string document_type { get; set; }

            public RequestContent()
            {

            }

        }

        private class ResponseContent
        {
            public string document_class { get; set; }
        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the DocumentClassifier class
        /// </summary>
        public DocumentClassifier()
        {
            client.BaseAddress = new Uri("https://text-document-classifier-4thir-kyc-custom-1.ai-sandbox.4th-ir.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            string requestUrl = "/api/v1/classify/";

            RequestContent request = new RequestContent();
            request.document_content = ReadDocumentContentBase64(path);
            request.document_type = documentType.ToString();

            var requestJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUrl, requestJson);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();

                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r);

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
