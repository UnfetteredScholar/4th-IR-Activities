using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextSummarization.BigBird;
using System.IO;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextSummarizationBB_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextSummarizationBB_Description))]
    public class TextSummarizationBB : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextSummarizationBB_Article_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarizationBB_Article_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Article { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextSummarizationBB_Summary_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarizationBB_Summary_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Summary { get; set; }

        #endregion


        #region Constructors

        public TextSummarizationBB()
        {
        }

        #endregion


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
            };
        }

        private async Task<Tuple<string, string>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var article = Article.Get(context);
            TextSummarizerBB summarizerBB = new TextSummarizerBB(ProcessScope.GetHttpClient());

            Tuple<string, string> res;

            if (File.Exists(article))
            {
                res = await summarizerBB.SummarizeTextFile(article);
            }
            else
            {
                res = await summarizerBB.SummarizeText(article);
            }

            return res;
        }

        #endregion
    }
}

