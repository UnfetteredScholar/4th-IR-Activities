using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.HGenerateTS.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.HGenerateTS.TextSummarization.HeadlineGeneration;

namespace _4thIR.HGenerateTS.Activities
{
    [LocalizedDisplayName(nameof(Resources.HeadlineGeneration_DisplayName))]
    [LocalizedDescription(nameof(Resources.HeadlineGeneration_Description))]
    public class HeadlineGeneration : ContinuableAsyncCodeActivity
    {
        private static readonly HeadlineGenerator _generator = new HeadlineGenerator();

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

        [LocalizedDisplayName(nameof(Resources.HeadlineGeneration_ArticleText_DisplayName))]
        [LocalizedDescription(nameof(Resources.HeadlineGeneration_ArticleText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ArticleText { get; set; }

        [LocalizedDisplayName(nameof(Resources.HeadlineGeneration_Headline_DisplayName))]
        [LocalizedDescription(nameof(Resources.HeadlineGeneration_Headline_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Headline { get; set; }

        #endregion


        #region Constructors

        public HeadlineGeneration()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (ArticleText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ArticleText)));

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
                Headline.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var articleText = ArticleText.Get(context);
            var res = await _generator.GenerateHeadline(articleText);

            return res;
        }

        #endregion
    }
}

