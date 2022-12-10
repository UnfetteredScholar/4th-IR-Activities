using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using NameEntityRecognition;
using NameEntityRecognition.Flair;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionFlair_DisplayName))]
    [LocalizedDescription(nameof(Resources.NameEntityRecognitionFlair_Description))]
    public class NameEntityRecognitionFlair : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionFlair_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognitionFlair_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionFlair_TextValues_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognitionFlair_TextValues_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<TextValuePair[]> TextValues { get; set; }

        #endregion


        #region Constructors

        public NameEntityRecognitionFlair()
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
                TextValues.Set(ctx, task.Result);
            };
        }

        private async Task<TextValuePair[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);
            NameEntityRecognizer recognizer = new NameEntityRecognizer(ProcessScope.GetHttpClient());

            var result = await recognizer.RecognizeNameEntity(sentence);

            return result;
        }

        #endregion
    }
}

