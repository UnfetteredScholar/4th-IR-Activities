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
    [LocalizedDisplayName(nameof(Resources.PDFParsing_DisplayName))]
    [LocalizedDescription(nameof(Resources.PDFParsing_Description))]
    public class PDFParsing : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.PDFParsing_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.PDFParsing_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.PDFParsing_NumberOfPages_DisplayName))]
        [LocalizedDescription(nameof(Resources.PDFParsing_NumberOfPages_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<int> NumberOfPages { get; set; }

        [LocalizedDisplayName(nameof(Resources.PDFParsing_Content_DisplayName))]
        [LocalizedDescription(nameof(Resources.PDFParsing_Content_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string[]> Content { get; set; }

        #endregion


        #region Constructors

        public PDFParsing()
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
                NumberOfPages.Set(ctx, task.Result.numberOfPages);
                Content.Set(ctx, task.Result.content);
            };
        }

        private async Task<(int numberOfPages, string[] content)> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var res = await _textUtility.ParsePDF(path);

            return res;
        }

        #endregion
    }
}

