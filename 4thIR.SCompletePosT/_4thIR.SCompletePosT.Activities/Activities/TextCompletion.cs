using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.SCompletePosT.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.SCompletePosT.SentenceCompletion.TransformerXL;

namespace _4thIR.SCompletePosT.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextCompletion_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextCompletion_Description))]
    public class TextCompletion : ContinuableAsyncCodeActivity
    {
        private static readonly SentenceCompleter _completer = new SentenceCompleter();

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

        [LocalizedDisplayName(nameof(Resources.TextCompletion_IncompleteText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextCompletion_IncompleteText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> IncompleteText { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextCompletion_AddedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextCompletion_AddedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> AddedText { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextCompletion_CompleteText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextCompletion_CompleteText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> CompleteText { get; set; }

        #endregion


        #region Constructors

        public TextCompletion()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (IncompleteText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(IncompleteText)));

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
                AddedText.Set(ctx, task.Result.Item1);
                CompleteText.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string,string>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var incompleteText = IncompleteText.Get(context);
            var res = await _completer.CompleteText(incompleteText);

            return res;
        }

        #endregion
    }
}

