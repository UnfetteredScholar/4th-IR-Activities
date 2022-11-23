using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.LTranslate.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextTranslation.OpenVino;


namespace _4thIR.LTranslate.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextTranslation_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextTranslation_Description))]
    public class TextTranslation : ContinuableAsyncCodeActivity
    {
        private static readonly TextTranslator _translator = new TextTranslator();

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

        [LocalizedDisplayName(nameof(Resources.TextTranslation_Sentences_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_Sentences_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> Sentences { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_TranslationType_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_TranslationType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<TransationType> TranslationType { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_TranslatedSentences_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_TranslatedSentences_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<TranslatedText[]> TranslatedSentences { get; set; }

        #endregion


        #region Constructors

        public TextTranslation()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentences == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentences)));
            if (TranslationType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TranslationType)));

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
                TranslatedSentences.Set(ctx, task.Result);
            };
        }

        private async Task<TranslatedText[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentences = Sentences.Get(context);
            var translationType = TranslationType.Get(context);
            var res = await _translator.TranslateText(sentences, translationType);

            return res;
        }

        #endregion
    }
}

