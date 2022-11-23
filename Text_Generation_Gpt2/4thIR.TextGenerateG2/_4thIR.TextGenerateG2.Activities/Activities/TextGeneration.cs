using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextGenerateG2.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextGeneration.Gpt2;

namespace _4thIR.TextGenerateG2.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextGeneration_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextGeneration_Description))]
    public class TextGeneration : ContinuableAsyncCodeActivity
    {
        private static readonly TextGeneratorGpt2 _generator = new();

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

        [LocalizedDisplayName(nameof(Resources.TextGeneration_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGeneration_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextGeneration_GeneratedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGeneration_GeneratedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string[]> GeneratedText { get; set; }

        #endregion


        #region Constructors

        public TextGeneration()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Text == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));

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
                GeneratedText.Set(ctx, task.Result);
            };
        }

        private async Task<string[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var text = Text.Get(context);
            var res = await _generator.GenerateText(text);

            return res;
        }

        #endregion
    }
}

