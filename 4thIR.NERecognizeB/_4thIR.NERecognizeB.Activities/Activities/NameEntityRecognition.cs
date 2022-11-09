using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using _4thIR.NERecognizeB.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using NameEntityRecognition.BERT;

namespace _4thIR.NERecognizeB.Activities
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

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognition_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognition_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognition_TextValues_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognition_TextValues_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<TextValuePair[]> TextValues { get; set; }

        #endregion


        #region Constructors

        public NameEntityRecognition()
        {
        }

        #endregion

        private static readonly NameEntityRecognizerBERT recognizer = new NameEntityRecognizerBERT();
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
                TextValues.Set(ctx, task.Result);
            };
        }

        private async Task<TextValuePair[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var text = Text.Get(context);

            TextValuePair[] res;

            if(File.Exists(text))
            {
                res = await recognizer.RecognizeNameEntityInFile(text);
            }
            else
            {
                res = await recognizer.RecognizeNameEntity(text);
            }

            return res;
        }

        #endregion
    }
}

