using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using HandwrittenTextRecognition.OpenVino;
using System.ComponentModel;
using UiPath.Shared.Activities.Utilities;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.HandwrittenTextRecognitionOpenVino_DisplayName))]
    [LocalizedDescription(nameof(Resources.HandwrittenTextRecognitionOpenVino_Description))]
    public class HandwrittenTextRecognitionOpenVino : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.HandwrittenTextRecognitionOpenVino_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.HandwrittenTextRecognitionOpenVino_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.HandwrittenTextRecognitionOpenVino_Language_DisplayName))]
        [LocalizedDescription(nameof(Resources.HandwrittenTextRecognitionOpenVino_Language_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<Language>))]
        public Language Language { get; set; }

        [LocalizedDisplayName(nameof(Resources.HandwrittenTextRecognitionOpenVino_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.HandwrittenTextRecognitionOpenVino_Text_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Text { get; set; }

        #endregion


        #region Constructors

        public HandwrittenTextRecognitionOpenVino()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));
            if (Language == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Language)));

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
                Text.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);

            HandwrittenTextRecognizer textRecognizer = new HandwrittenTextRecognizer(ProcessScope.GetHttpClient());

            var res = await textRecognizer.DetectText(path,Language);

            return res;
        }

        #endregion
    }
}

