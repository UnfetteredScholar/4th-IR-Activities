using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.PartSpeechIdentify.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using PartOfScpeech.Tagging;

namespace _4thIR.PartSpeechIdentify.Activities
{
    [LocalizedDisplayName(nameof(Resources.PartOfSpeechIdentification_DisplayName))]
    [LocalizedDescription(nameof(Resources.PartOfSpeechIdentification_Description))]
    public class PartOfSpeechIdentification : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.PartOfSpeechIdentification_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.PartOfSpeechIdentification_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.PartOfSpeechIdentification_TextTag_DisplayName))]
        [LocalizedDescription(nameof(Resources.PartOfSpeechIdentification_TextTag_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<WordTag[]> TextTag { get; set; }

        #endregion


        #region Constructors

        public PartOfSpeechIdentification()
        {
        }

        #endregion

        private static readonly PartOfSpeechIdentifier identifier = new PartOfSpeechIdentifier();

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
                TextTag.Set(ctx, task.Result);
            };
        }

        private async Task<WordTag[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);

            var res=await identifier.IdentifyPartOfSpeech(sentence);

            return res;
        }

        #endregion
    }
}

