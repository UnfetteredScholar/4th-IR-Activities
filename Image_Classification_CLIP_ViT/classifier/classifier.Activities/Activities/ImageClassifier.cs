using System;
using System.Activities;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using classifier.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using System.Formats.Asn1;
using System.Net.Http.Json;

namespace classifier.Activities
{
    [LocalizedDisplayName(nameof(Resources.ImageClassifier_DisplayName))]
    [LocalizedDescription(nameof(Resources.ImageClassifier_Description))]
    public class ImageClassifier : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassifier_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassifier_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassifier_UserInput_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassifier_UserInput_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> UserInput { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassifier_Classificaton_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassifier_Classificaton_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Classificaton { get; set; }

        #endregion


        #region Constructors

        public ImageClassifier()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var filepath = FilePath.Get(context);
            var userinput = UserInput.Get(context);
            string apiUrl = "https://image-classification-clip-vit.ai-sandbox.4th-ir.io/api/v1/classify";

            client apiCall = new client(apiUrl, filepath, userinput);
            
            string res = await apiCall.UploadFile();
            

            // Outputs
            return (ctx) => {
                Classificaton.Set(ctx, res);
            };
        }

        #endregion
    }

    public class client
    {
        private HttpClient _client;
        private string _filePath;
        private string _apiUrl;
        private string _input;

        public client(string apiUrl, string filePath, string[] input)
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
                using(var httpClient = new HttpClient())
                {
                    using(var request = new HttpRequestMessage(new HttpMethod("POST"), _apiUrl))
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
            catch (Exception ex) {
                return ex.Message;
            }
             
        }
    }

    public class FileUploadResult
    {
        public string[] scores { get; set; }
    }
}

