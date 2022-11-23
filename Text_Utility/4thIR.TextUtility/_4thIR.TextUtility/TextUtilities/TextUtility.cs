using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TextUtilities.Exceptions;


namespace TextUtilities
{
    public enum Format { RawText, txt, pdf };

    public class TextUtility
    {
        private class PdfParseResponse
        {
            public PdfParseResponse()
            {

            }

            public object info { get; set; }
            public int num_pages { get; set; }
            public string[] content { get; set; }


        }

        private class TokenizeResponse
        {
            public TokenizeResponse()
            {

            }

            public string[] sentence { get; set; }
        }

        private class TextRecongitionResponse
        {
            public TextRecongitionResponse()
            {

            }

            public string text { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public TextUtility()
        {
            _client.BaseAddress = new Uri("https://text-utility.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string[]> TokenizeTextNlp(string text, Format format)
        {
            if (format != Format.RawText)
                text = @"" + text;

            using (var formData = new MultipartFormDataContent())
            {

                if (format == Format.RawText)
                {
                    StringContent stringContent = new StringContent(text, Encoding.UTF8, "text/plain");
                    formData.Add(stringContent, "sentence");
                }
                else
                {
                    int index = text.LastIndexOf('\\') + 1;
                    string fileName = text.Substring(index);

                    if (format == Format.txt)
                    {
                        StreamContent txtStream = new StreamContent(File.OpenRead(text));
                        txtStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("text/plain");
                        formData.Add(txtStream, "txt_file", fileName);
                    }
                    else if (format == Format.pdf)
                    {

                        StreamContent pdfStream = new StreamContent(File.OpenRead(text));
                        pdfStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/pdf");
                        formData.Add(pdfStream, "pdf", fileName);
                    }
                }

                string requestUri = "/api/v1/sentence/nltk_tokenize";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    TokenizeResponse tokenizeResponse = JsonSerializer.Deserialize<TokenizeResponse>(r);

                    return tokenizeResponse.sentence;

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

                    throw new TextUtilityException(message, ex);
                }
            }
        }

        public async Task<string[]> TokenizeTextSpacy(string text, Format format)
        {
            if (format != Format.RawText)
                text = @"" + text;

            using (var formData = new MultipartFormDataContent())
            {

                if (format == Format.RawText)
                {
                    StringContent stringContent = new StringContent(text, Encoding.UTF8, "text/plain");
                    formData.Add(stringContent, "sentence");
                }
                else
                {
                    int index = text.LastIndexOf('\\') + 1;
                    string fileName = text.Substring(index);

                    if (format == Format.txt)
                    {
                        StreamContent txtStream = new StreamContent(File.OpenRead(text));
                        txtStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("text/plain");
                        formData.Add(txtStream, "sentence_in_file", fileName);
                    }
                    else if (format == Format.pdf)
                    {

                        StreamContent pdfStream = new StreamContent(File.OpenRead(text));
                        pdfStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/pdf");
                        formData.Add(pdfStream, "pdf_file", fileName);
                    }
                }

                string requestUri = "/api/v1/sentence/spacy_tokenize";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    TokenizeResponse tokenizeResponse = JsonSerializer.Deserialize<TokenizeResponse>(r);

                    return tokenizeResponse.sentence;

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

                    throw new TextUtilityException(message, ex);
                }
            }
        }

        public async Task<string> RecognizeTextImage(string path)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {

                StreamContent imageStream = new StreamContent(File.OpenRead(path));
                imageStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/*");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(imageStream, "image_file", fileName);

                string requestUri = "/api/v1/ocr/image";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    TextRecongitionResponse textRecongitionResponse = JsonSerializer.Deserialize<TextRecongitionResponse>(r);

                    return textRecongitionResponse.text;

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
                        message = "Error: Unable to read text";
                    }

                    throw new TextUtilityException(message, ex);
                }
            }

        }

        public async Task<string> RecognizeTextPDF(string path)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {

                StreamContent docStream = new StreamContent(File.OpenRead(path));
                docStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/pdf");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(docStream, "pdf_file", fileName);

                string requestUri = "/api/v1/ocr/image";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    TextRecongitionResponse textRecongitionResponse = JsonSerializer.Deserialize<TextRecongitionResponse>(r);

                    return textRecongitionResponse.text;

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
                        message = "Error: Unable to read text";
                    }

                    throw new TextUtilityException(message, ex);
                }
            }

        }

        public async Task<(int numberOfPages, string[] content)> ParsePDF(string path)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {

                StreamContent docStream = new StreamContent(File.OpenRead(path));
                docStream.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/pdf");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);

                formData.Add(docStream, "pdf_file", fileName);

                string requestUri = "/api/v1/pdf";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    string r = await response.Content.ReadAsStringAsync();
                    PdfParseResponse parseResponse = JsonSerializer.Deserialize<PdfParseResponse>(r);

                    return new(parseResponse.num_pages, parseResponse.content);

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
                        message = "Error: Unable to parse pdf";
                    }

                    throw new TextUtilityException(message, ex);
                }
            }

        }
    }
}
