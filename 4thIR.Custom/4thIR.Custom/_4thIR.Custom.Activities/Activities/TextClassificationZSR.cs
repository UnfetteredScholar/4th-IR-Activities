using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextClassification.ZeroShot.Roberta;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextClassificationZSR_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextClassificationZSR_Description))]
    public class TextClassificationZSR : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextClassificationZSR_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextClassificationZSR_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextClassificationZSR_Labels_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextClassificationZSR_Labels_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> Labels { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextClassificationZSR_LabelScores_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextClassificationZSR_LabelScores_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<LabelScorePair[]> LabelScores { get; set; }

        #endregion


        #region Constructors

        public TextClassificationZSR()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Text == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));
            if (Labels == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Labels)));

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
                LabelScores.Set(ctx, task.Result);
            };
        }

        private async Task<LabelScorePair[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var text = Text.Get(context);
            var labels = Labels.Get(context);

            TextClassifierZSR classifier = new TextClassifierZSR(ProcessScope.GetHttpClient());

            var res = await classifier.ClassifyText(text, labels);

            return res;
        }

        #endregion
    }
}

