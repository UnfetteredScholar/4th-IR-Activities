using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextSummarization.Bart.large;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextSummarizerBartLarge_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextSummarizerBartLarge_Description))]
    public class TextSummarizerBartLarge : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextSummarizerBartLarge_UserSentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarizerBartLarge_UserSentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> UserSentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextSummarizerBartLarge_Summary_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextSummarizerBartLarge_Summary_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Summary { get; set; }

        #endregion


        #region Constructors

        public TextSummarizerBartLarge()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (UserSentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(UserSentence)));

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
                Summary.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var usersentence = UserSentence.Get(context);

            TextSummarizerBart summarizer = new TextSummarizerBart(usersentence, ProcessScope.GetHttpClient());
            var res = await summarizer.summarize();

            return res;

        }

        #endregion
    }
}

