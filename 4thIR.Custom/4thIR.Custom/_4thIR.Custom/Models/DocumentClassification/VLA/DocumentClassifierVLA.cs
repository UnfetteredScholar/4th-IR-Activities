using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DocumentClassification.Exceptions;

namespace DocumentClassification.VLA
{
    /// <summary>
    /// Stores data relating to a document
    /// </summary>
    public class DocumentInfo
    {
        public DocumentInfo(string documentName, string documentClass, string documentText, DocumentMeta documentMeta)
        {
            DocumentName = documentName;
            DocumentClass = documentClass;
            DocumentText = documentText;
            DocumentMeta = documentMeta;

        }

        /// <summary>
        /// The name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// The label or document class
        /// </summary>
        public string DocumentClass { get; set; }

        /// <summary>
        /// The document content
        /// </summary>
        public string DocumentText { get; set; }

        /// <summary>
        /// The document meta data
        /// </summary>
        public DocumentMeta DocumentMeta { get; set; }
    }

    /// <summary>
    /// Stores the meta data of a document
    /// </summary>
    public class DocumentMeta
    {
        public DocumentMeta(string siteID, string startDate, string endDate, string[] relatedParties)
        {
            SiteID = siteID;
            StartDate = startDate;
            EndDate = endDate;
            RelatedParties = relatedParties;
        }

        public string SiteID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string[] RelatedParties { get; set; }
    }


    public class DocumentClassifierVLA
    {
        private class ResponseDocumentMeta
        {
            public ResponseDocumentMeta()
            {

            }

            public string site_id { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string[] related_parties { get; set; }
        }

        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string document_name { get; set; }
            public string document_class { get; set; }
            public string document_text { get; set; }
            public ResponseDocumentMeta document_meta { get; set; }
        }

        private HttpClient _client = null;

        public DocumentClassifierVLA(HttpClient client)
        {
            _client = client;
            //_client.BaseAddress = new Uri("https://text-document-classification-vla-2.ai-sandbox.4th-ir.io");
            // _client.DefaultRequestHeaders.Accept.Clear();
            // _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DocumentInfo> ClassifyDocument(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent documentStream = new StreamContent(File.OpenRead(path));
                    documentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    string fileName = Path.GetFileName(path);

                    formData.Add(documentStream, "pdf_file", fileName);

                    string requestUri = "https://text-document-classification-vla-2.ai-sandbox.4th-ir.io/api/v1/classify/";
                    response = await _client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    DocumentMeta documentMeta = null;

                    if (responseContent.document_meta != null)
                        documentMeta = new DocumentMeta(responseContent.document_meta.site_id, responseContent.document_meta.start_date, responseContent.document_meta.end_date, responseContent.document_meta.related_parties);

                    DocumentInfo documentInfo = new DocumentInfo(responseContent.document_name, responseContent.document_class, responseContent.document_text, documentMeta);

                    return documentInfo;
                }
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

                throw new DocumentClassificationException(message, ex);
            }
        }


        public async Task<DocumentInfo> ExtractDocumentMeta(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent documentStream = new StreamContent(File.OpenRead(path));
                    documentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    string fileName = Path.GetFileName(path);

                    formData.Add(documentStream, "pdf_file", fileName);

                    string requestUri = "https://text-document-classification-vla-2.ai-sandbox.4th-ir.io/api/v1/extract-meta-data/";
                    response = await _client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    DocumentMeta documentMeta = null;

                    if (responseContent.document_meta != null)
                        documentMeta = new DocumentMeta(responseContent.document_meta.site_id, responseContent.document_meta.start_date, responseContent.document_meta.end_date, responseContent.document_meta.related_parties);

                    DocumentInfo documentInfo = new DocumentInfo(responseContent.document_name, responseContent.document_class, responseContent.document_text, documentMeta);

                    return documentInfo;
                }
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

                throw new DocumentClassificationException(message, ex);
            }
        }


        public async Task<DocumentInfo> ClassifyAndExtracttMeta(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent documentStream = new StreamContent(File.OpenRead(path));
                    documentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    string fileName = Path.GetFileName(path);

                    formData.Add(documentStream, "pdf_file", fileName);

                    string requestUri = "https://text-document-classification-vla-2.ai-sandbox.4th-ir.io/api/v1/classify-and-extract-meta-data/";
                    response = await _client.PostAsync(requestUri, formData);


                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    DocumentMeta documentMeta = new DocumentMeta(responseContent.document_meta.site_id, responseContent.document_meta.start_date, responseContent.document_meta.end_date, responseContent.document_meta.related_parties);

                    DocumentInfo documentInfo = new DocumentInfo(responseContent.document_name, responseContent.document_class, responseContent.document_text, documentMeta);

                    return documentInfo;
                }
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

                throw new DocumentClassificationException(message, ex);
            }
        }


        public async Task<DocumentInfo> TrueDigitalClassifyAndExtracttMeta(string documentName, string documentText)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var requestContent = new { document_text = documentText, document_name = documentName };

                string requestUri = "https://text-document-classification-vla-2.ai-sandbox.4th-ir.io/api/v1/true-digital-classify-and-extract-meta-data/";
                response = await _client.PostAsync(requestUri, new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                DocumentMeta documentMeta = new DocumentMeta(responseContent.document_meta.site_id, responseContent.document_meta.start_date, responseContent.document_meta.end_date, responseContent.document_meta.related_parties);

                DocumentInfo documentInfo = new DocumentInfo(responseContent.document_name, responseContent.document_class, responseContent.document_text, documentMeta);

                return documentInfo;
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

                throw new DocumentClassificationException(message, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>(Document Name, Document Text)</returns>
        /// <exception cref="DocumentClassificationException"></exception>
        public async Task<Tuple<string, string>> ConvertToTrueDigital(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                path = @"" + path;

                using (var formData = new MultipartFormDataContent())
                {
                    StreamContent documentStream = new StreamContent(File.OpenRead(path));
                    documentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    string fileName = Path.GetFileName(path);

                    formData.Add(documentStream, "pdf_file", fileName);

                    string requestUri = "https://text-document-classification-vla-2.ai-sandbox.4th-ir.io/api/v1/convert-to-true-digital-v2/";
                    response = await _client.PostAsync(requestUri, formData);

                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r);

                    return new Tuple<string, string>(responseContent.document_name, responseContent.document_text);
                }
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

                throw new DocumentClassificationException(message, ex);
            }
        }
    }

}

