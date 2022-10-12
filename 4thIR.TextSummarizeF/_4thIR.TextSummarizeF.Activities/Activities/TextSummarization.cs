using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextSummarizeF.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextSummarizationFairseq;

namespace _4thIR.TextSummarizeF.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextSummarization_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextSummarization_Description))]
    public class TextSummarization : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.TextSummarization_Article_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarization_Article_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Article { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextSummarization_Summary_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarization_Summary_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Summary { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextSummarization_Model_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarization_Model_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Model { get; set; }

        #endregion


        #region Constructors

        public TextSummarization()
        {
        }

        #endregion

        private static readonly TextSummarizerFairseq summarizer = new TextSummarizerFairseq();

        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Article == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Article)));

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
                Summary.Set(ctx, task.Result.Item1);
                Model.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string,string>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var article = Article.Get(context);

            var res = await summarizer.SummarizeText(article);

            return res;
        }

        #endregion
    }
}

