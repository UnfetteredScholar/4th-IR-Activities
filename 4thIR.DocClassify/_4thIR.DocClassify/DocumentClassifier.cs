using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

namespace DocumentClassification
{
    /// <summary>
    /// Document Type
    /// </summary>
    public enum DocumentType { pdf, image}

    /// <summary>
    /// Document Class
    /// </summary>
    public enum DocumentClass { passport, commercial_register, other}

    public class DocumentClassifier
    {
        private static readonly HttpClient client = new HttpClient();

        public DocumentClassifier()
        {
            //client.BaseAddress = new System.Uri("https://text-document-classifier-4thir-kyc-custom-1.ai-sandbox.4th-ir.io/api/v1/classify/");
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
            Byte[] bytes = File.ReadAllBytes(path);
            string file = Convert.ToBase64String(bytes);

            return file;
        }

        /// <summary>
        /// Classifies documents based on their content
        /// </summary>
        /// <param name="documentContent">Base64 encoded string of file</param>
        /// <param name="documentType">File type</param>
        /// <returns>Document class {passport, commercial register, other}</returns>
        public async Task<string> ClassifyDocument(string documentContent, DocumentType documentType)
        {
            string requestUrl = "https://text-document-classifier-4thir-kyc-custom-1.ai-sandbox.4th-ir.io/api/v1/classify/";
            RequestContent request = new RequestContent();
            request.document_content=documentContent;  
            request.document_type=documentType.ToString();

            var requestJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,"application/json");
            //Console.WriteLine(JsonConvert.SerializeObject(request));
            var response = await client.PostAsync(requestUrl, requestJson);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] param = { '[', ']' };
                ResponseContent responseContent = JsonConvert.DeserializeObject<ResponseContent>(r.Trim(param));

                return responseContent?.document_class != null ? responseContent.document_class : " ";
            }
            catch(HttpRequestException ex)
            {
                string message = "";

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    message= "Error processing document_content";
                }
                else
                {
                    message= "Error";
                }

                throw new Exception(message,ex);
            }

           

        }

    }
}
