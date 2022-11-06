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
    [LocalizedDisplayName(nameof(Resources.DocumentTextAnalysis_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentTextAnalysis_Description))]
    public class DocumentTextAnalysis : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.DocumentTextAnalysis_DocumentName_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextAnalysis_DocumentName_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> DocumentName { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentTextAnalysis_DocumentText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextAnalysis_DocumentText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> DocumentText { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentTextAnalysis_DocumentInfo_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentTextAnalysis_DocumentInfo_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<DocumentInfo> DocumentInfo { get; set; }

        #endregion


        #region Constructors

        public DocumentTextAnalysis()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (DocumentName == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DocumentName)));
            if (DocumentText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DocumentText)));

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
                DocumentInfo.Set(ctx, task.Result);
            };
        }

        private async Task<DocumentInfo> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var documentName = DocumentName.Get(context);
            var documentText = DocumentText.Get(context);

            var res = await _classifier.TrueDigitalClassifyAndExtracttMeta(documentName, documentText);

            return res;
        }

        #endregion
    }
}

