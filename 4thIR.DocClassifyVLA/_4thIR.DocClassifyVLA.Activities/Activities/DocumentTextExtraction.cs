using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.DocClassifyVLA.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.DocClassifyVLA.DocumentClassification.VLA;

namespace _4thIR.DocClassifyVLA.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentTextExtraction_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentTextExtraction_Description))]
    public class DocumentTextExtraction : ContinuableAsyncCodeActivity
    {
        private static readonly DocumentClassifierVLA _classifier = new DocumentClassifierVLA();
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

        [LocalizedDisplayName(nameof(Resources.DocumentTextExtraction_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextExtraction_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentTextExtraction_DocumentName_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextExtraction_DocumentName_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> DocumentName { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentTextExtraction_DocumentText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextExtraction_DocumentText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> DocumentText { get; set; }

        #endregion


        #region Constructors

        public DocumentTextExtraction()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);
            

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                DocumentName.Set(ctx, task.Result.documentName);
                DocumentText.Set(ctx, task.Result.documentText);
            };
        }

        private async Task<(string documentName, string documentText)> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);

            var res=await _classifier.ConvertToTrueDigital(path);

            return res;
        }

        #endregion
    }
}

