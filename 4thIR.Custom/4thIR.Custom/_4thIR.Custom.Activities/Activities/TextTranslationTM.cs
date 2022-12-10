using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextTranslation.Multilanguage;
using System.ComponentModel;
using UiPath.Shared.Activities.Utilities;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextTranslationTM_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextTranslationTM_Description))]
    public class TextTranslationTM : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextTranslationTM_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslationTM_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslationTM_TranslatedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslationTM_TranslatedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> TranslatedText { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslationTM_SourceLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslationTM_SourceLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<Language>))]
        public Language SourceLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslationTM_ConversionLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslationTM_ConversionLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<Language>))]
        public Language ConversionLanguage { get; set; }

        #endregion


        #region Constructors

        public TextTranslationTM()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentence)));
            if (SourceLanguage == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceLanguage)));
            if (ConversionLanguage == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ConversionLanguage)));
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
            var sentence = Sentence.Get(context);
            Translator translator = new Translator(ProcessScope.GetHttpClient());

            var res = await translator.TranslateText(sentence, SourceLanguage, ConversionLanguage);

            return res;
        }

        #endregion
    }
}

