﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using _4thIR.QAnswerB.QuestionAnswering.Exceptions;

namespace _4thIR.QAnswerB.QuestionAnswering.Bert
{
    public class QuestionAnswererBert
    {
        private class ResponseContent
        {
            public ResponseContent()
            {

            }

            public string answer { get; set; }
            public string score { get; set; }
            public string context { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public QuestionAnswererBert()
        {
            _client.BaseAddress = new Uri("https://text-document-question-answer-bert-large.ai-sandbox.4th-ir.io");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Answers questions based on the content of a document
        /// </summary>
        /// <param name="path">Full path to pdf document</param>
        /// <param name="question">Question to be answered</param>
        /// <returns>Tuple(Answer,Score,Context)</returns>
        /// <exception cref="QuestionAnsweringException"></exception>
        public async Task<Tuple<string, double, string>> AnswerQuestion(string path, string question)
        {
            path = @"" + path;

            using (var formData = new MultipartFormDataContent())
            {
                StreamContent documentStream = new StreamContent(File.OpenRead(path));
                documentStream.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                int index = path.LastIndexOf('\\') + 1;
                string fileName = path.Substring(index);
                formData.Add(documentStream, "pdf_file", fileName);

                StringContent questionString = new StringContent(question, Encoding.UTF8, "text/plain");
                formData.Add(questionString, "questions");

                string requestUri = "/api/v1/answer";
                var response = await _client.PostAsync(requestUri, formData);

                try
                {
                    response.EnsureSuccessStatusCode();

                    var r = await response.Content.ReadAsStringAsync();
                    ResponseContent[] answers = JsonSerializer.Deserialize<ResponseContent[]>(r);

                    return new Tuple<string, double, string>(answers[0].answer, double.Parse(answers[0].score), answers[0].context);

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

                    throw new QuestionAnsweringException(message, ex);
                }

            }
        }
    }
}
