using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextTranslationT.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextTranslation.MachineTranslation.Transformers;

namespace _4thIR.TextTranslationT.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextTranslation_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextTranslation_Description))]
    public class TextTranslation : ContinuableAsyncCodeActivity
    {
        private static readonly TextTranslatorT translator = new TextTranslatorT();

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

        [LocalizedDisplayName(nameof(Resources.TextTranslation_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_SourceLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_SourceLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Language> SourceLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_ConversionLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_ConversionLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Language> ConversionLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_ConvertedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_ConvertedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> ConvertedText { get; set; }

        #endregion


        #region Constructors

        public TextTranslation()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentence)));
            if (SourceLanguage == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceLanguage)));
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
                ConvertedText.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);
            var sourceLanguage = SourceLanguage.Get(context);
            var conversionLanguage = ConversionLanguage.Get(context);

            var res = await translator.TranslateText(sentence, sourceLanguage, conversionLanguage);

            return res;

        }

        #endregion
    }
}

