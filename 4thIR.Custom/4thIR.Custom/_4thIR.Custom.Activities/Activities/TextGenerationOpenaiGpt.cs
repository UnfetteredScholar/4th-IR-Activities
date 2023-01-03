using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextGeneration.OpenaiGpt;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextGenerationOpenaiGpt_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextGenerationOpenaiGpt_Description))]
    public class TextGenerationOpenaiGpt : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextGenerationOpenaiGpt_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGenerationOpenaiGpt_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextGenerationOpenaiGpt_GeneratedTexts_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGenerationOpenaiGpt_GeneratedTexts_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string[]> GeneratedTexts { get; set; }

        #endregion


        #region Constructors

        public TextGenerationOpenaiGpt()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentence)));

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
                GeneratedTexts.Set(ctx, task.Result);
            };
        }

        private async Task<string[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);

            TextGeneratorOG generator = new TextGeneratorOG(ProcessScope.GetHttpClient());

            var res = await generator.GenerateText(sentence);

            return res;
        }

        #endregion
    }
}

