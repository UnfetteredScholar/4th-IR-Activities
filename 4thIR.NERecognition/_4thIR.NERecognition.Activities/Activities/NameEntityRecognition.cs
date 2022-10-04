using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.NERecognition.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using NameEntityRecognition;
using System.Runtime.Remoting.Messaging;

namespace _4thIR.NERecognition.Activities
{
    [LocalizedDisplayName(nameof(Resources.NameEntityRecognition_DisplayName))]
    [LocalizedDescription(nameof(Resources.NameEntityRecognition_Description))]
    public class NameEntityRecognition : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognition_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognition_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognition_TextValue_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognition_TextValue_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<WordTag[]> TextValue { get; set; }

        #endregion


        #region Constructors

        public NameEntityRecognition()
        {
        }

        #endregion

        private static readonly NameEntityRecognizer recognizer = new NameEntityRecognizer();

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
                TextValue.Set(ctx, task.Result);
            };
        }

        private async Task<WordTag[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);
            
            var result= await recognizer.RecognizeNameEntity(sentence);

            return result;
        }

        #endregion
    }
}

