using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextComparison.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.ProcessScope.Activities.Activities;
using TextComparison.AllMPnetBase.V2;

namespace _4thIR.TextComparison.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextComparison_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextComparison_Description))]
    public class TextComparison : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextComparison_SourceText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextComparison_SourceText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> SourceText { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextComparison_Sentences_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextComparison_Sentences_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> Sentences { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextComparison_Comparisons_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextComparison_Comparisons_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string[]> Comparisons { get; set; }

        #endregion


        #region Constructors

        public TextComparison()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SourceText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceText)));
            if (Sentences == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentences)));

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
                Comparisons.Set(ctx, task.Result);
            };
        }

        private async Task<string[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sourceText = SourceText.Get(context);
            var sentences = Sentences.Get(context);

            TextComparer comparer = new TextComparer(ProcessScope.Activities.ProcessScope.GetHttpClient());

            var res = await comparer.CompareText(sourceText, sentences);

            return res;
        }

        #endregion
    }
}

