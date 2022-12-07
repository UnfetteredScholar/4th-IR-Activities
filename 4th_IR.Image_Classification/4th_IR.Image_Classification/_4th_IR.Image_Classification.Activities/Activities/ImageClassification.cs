using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4th_IR.Image_Classification.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace _4th_IR.Image_Classification.Activities
{
    [LocalizedDisplayName(nameof(Resources.ImageClassification_DisplayName))]
    [LocalizedDescription(nameof(Resources.ImageClassification_Description))]
    public class ImageClassification : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.ImageClassification_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassification_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassification_UserInput_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassification_UserInput_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> UserInput { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassification_Classification_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassification_Classification_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Classification { get; set; }

        #endregion


        #region Constructors

        public ImageClassification()
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
            var timeout = TimeoutMS.Get(context);
            var filepath = FilePath.Get(context);
            var userinput = UserInput.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Classification.Set(ctx, task.Result);
            };
        }

        private async Task<String> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var filepath = FilePath.Get(context);
            var userinput = UserInput.Get(context);

            string apiUrl = "https://image-classification-clip-vit.ai-sandbox.4th-ir.io/api/v1/classify";

            Client apiCall = new Client(apiUrl, filepath, userinput);

            string res = await apiCall.UploadFile();

            return res;
        }

        #endregion
    }
}

