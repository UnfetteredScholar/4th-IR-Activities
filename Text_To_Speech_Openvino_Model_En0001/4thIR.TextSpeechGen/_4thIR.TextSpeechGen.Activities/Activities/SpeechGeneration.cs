using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextSpeechGen.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextToSpeech.Openvino;

namespace _4thIR.TextSpeechGen.Activities
{
    [LocalizedDisplayName(nameof(Resources.SpeechGeneration_DisplayName))]
    [LocalizedDescription(nameof(Resources.SpeechGeneration_Description))]
    public class SpeechGeneration : ContinuableAsyncCodeActivity
    {
        private static readonly TextToSpeechGenerator _speechGenerator=new TextToSpeechGenerator();

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

        [LocalizedDisplayName(nameof(Resources.SpeechGeneration_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechGeneration_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.SpeechGeneration_OutputFolder_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechGeneration_OutputFolder_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> OutputFolder { get; set; } = "";

        #endregion


        #region Constructors

        public SpeechGeneration()
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
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var text = Text.Get(context);
            var outputFolder = OutputFolder.Get(context);
            await _speechGenerator.GenerateAudio(text, outputFolder);
        }

        #endregion
    }
}

