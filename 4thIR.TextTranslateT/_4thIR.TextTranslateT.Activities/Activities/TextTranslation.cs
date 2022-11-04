using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.TextTranslateT.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.TextTranslateT.TextTranslation.Transformers;

namespace _4thIR.TextTranslateT.Activities
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

        [LocalizedDisplayName(nameof(Resources.TextTranslation_SourceLanguage_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_SourceLanguage_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Language> SourceLanguage { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextTranslation_OriginalText_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextTranslation_OriginalText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> OriginalText { get; set; }

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
            if (SourceLanguage == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceLanguage)));
            if (OriginalText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(OriginalText)));
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
                ConvertedText.Set(ctx, task.Result.conversionText);
            };
        }

        private async Task<(string originalText, string conversionText)> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sourceLanguage = SourceLanguage.Get(context);
            var originalText = OriginalText.Get(context);
            var conversionLanguage = ConversionLanguage.Get(context);
            var res=await _translator.TranslateText(originalText,sourceLanguage,conversionLanguage);

            return res;
        }

        #endregion
    }
}

