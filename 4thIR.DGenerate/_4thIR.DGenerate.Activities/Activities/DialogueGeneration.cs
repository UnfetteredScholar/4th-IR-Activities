using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.DGenerate.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.DGenerate.TextGenerarion.MultiTurnConversation;

namespace _4thIR.DGenerate.Activities
{
    [LocalizedDisplayName(nameof(Resources.DialogueGeneration_DisplayName))]
    [LocalizedDescription(nameof(Resources.DialogueGeneration_Description))]
    public class DialogueGeneration : ContinuableAsyncCodeActivity
    {
        private static readonly DialogueGenerator _dialogueGenerator=new DialogueGenerator();

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

        [LocalizedDisplayName(nameof(Resources.DialogueGeneration_Statement_DisplayName))]
        [LocalizedDescription(nameof(Resources.DialogueGeneration_Statement_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Statement { get; set; }

        [LocalizedDisplayName(nameof(Resources.DialogueGeneration_Response_DisplayName))]
        [LocalizedDescription(nameof(Resources.DialogueGeneration_Response_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Response { get; set; }

        #endregion


        #region Constructors

        public DialogueGeneration()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Statement == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Statement)));

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
                Response.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var statement = Statement.Get(context);

            var res = await _dialogueGenerator.GenerateDialogueResponse(statement);

            return res;
        }

        #endregion
    }
}

