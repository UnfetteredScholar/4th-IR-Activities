using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextUtility.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextUtilities;

namespace _4thIR.TextUtility.Activities
{
    [LocalizedDisplayName(nameof(Resources.PDFTextExtraction_DisplayName))]
    [LocalizedDescription(nameof(Resources.PDFTextExtraction_Description))]
    public class PDFTextExtraction : ContinuableAsyncCodeActivity
    {
        private static readonly TextUtilities.TextUtility _textUtility = new TextUtilities.TextUtility();
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

        [LocalizedDisplayName(nameof(Resources.PDFTextExtraction_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.PDFTextExtraction_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.PDFTextExtraction_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.PDFTextExtraction_Text_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Text { get; set; }

        #endregion


        #region Constructors

        public PDFTextExtraction()
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
                Text.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var res = await _textUtility.RecognizeTextPDF(path);

            return res;
        }

        #endregion
    }
}

