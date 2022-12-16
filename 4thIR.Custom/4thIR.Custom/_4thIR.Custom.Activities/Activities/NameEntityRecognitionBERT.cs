using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using NameEntityRecognition.BERT;
using System.ComponentModel;
using UiPath.Shared.Activities.Utilities;
using NameEntityRecognition;

namespace _4thIR.Custom.Activities
{
    public enum InputType { [Description("Select An Item")] SelectAnItem, [Description("Raw Text")] RawText, [Description("File")] File };

    [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionBERT_DisplayName))]
    [LocalizedDescription(nameof(Resources.NameEntityRecognitionBERT_Description))]
    public class NameEntityRecognitionBERT : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionBERT_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognitionBERT_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionBERT_TextValues_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognitionBERT_TextValues_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<TextValuePair[]> TextValues { get; set; }

        [LocalizedDisplayName(nameof(Resources.NameEntityRecognitionBERT_InputType_DisplayName))]
        [LocalizedDescription(nameof(Resources.NameEntityRecognitionBERT_InputType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<InputType>))]
        public InputType InputType { get; set; }

        #endregion


        #region Constructors

        public NameEntityRecognitionBERT()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Text == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));
            if (InputType == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));

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

            NameEntityRecognizerBERT recognizer = new NameEntityRecognizerBERT(ProcessScope.GetHttpClient());
            TextValuePair[] res;

            if(InputType==InputType.RawText)
                res=await recognizer.RecognizeNameEntity(text);
            else
                res=await recognizer.RecognizeNameEntityInFile(text);

            return res;

        }

        #endregion
    }
}

