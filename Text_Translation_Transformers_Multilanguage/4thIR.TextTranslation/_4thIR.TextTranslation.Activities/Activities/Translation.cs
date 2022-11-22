using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextTranslation.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextTranslation.Multilanguage;

namespace _4thIR.TextTranslation.Activities
{
    [LocalizedDisplayName(nameof(Resources.Translation_DisplayName))]
    [LocalizedDescription(nameof(Resources.Translation_Description))]
    public class Translation : ContinuableAsyncCodeActivity
    {
        private static readonly Translator translator = new Translator();

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

        [LocalizedDisplayName(nameof(Resources.Translation_SourceLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.Translation_SourceLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Language> SourceLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.Translation_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.Translation_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.Translation_ConversionLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.Translation_ConversionLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Language> ConversionLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.Translation_TranslatedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.Translation_TranslatedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> TranslatedText { get; set; }

        #endregion


        #region Constructors

        public Translation()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SourceLanguage == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceLanguage)));
            if (Sentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentence)));
            if (ConversionLanguage == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ConversionLanguage)));

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
                TranslatedText.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sourceLanguage = SourceLanguage.Get(context);
            var sentence = Sentence.Get(context);
            var conversionLanguage = ConversionLanguage.Get(context);

            var res = await translator.TranslateText(sentence, sourceLanguage, conversionLanguage);

            return res;
        }

        #endregion
    }
}

