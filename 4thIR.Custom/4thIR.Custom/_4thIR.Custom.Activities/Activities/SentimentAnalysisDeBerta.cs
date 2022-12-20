using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using SentimentAnalysis.DeBERTA;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.SentimentAnalysisDeBerta_DisplayName))]
    [LocalizedDescription(nameof(Resources.SentimentAnalysisDeBerta_Description))]
    public class SentimentAnalysisDeBerta : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.SentimentAnalysisDeBerta_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.SentimentAnalysisDeBerta_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.SentimentAnalysisDeBerta_Label_DisplayName))]
        [LocalizedDescription(nameof(Resources.SentimentAnalysisDeBerta_Label_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Label { get; set; }

        [LocalizedDisplayName(nameof(Resources.SentimentAnalysisDeBerta_Score_DisplayName))]
        [LocalizedDescription(nameof(Resources.SentimentAnalysisDeBerta_Score_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double> Score { get; set; }

        #endregion


        #region Constructors

        public SentimentAnalysisDeBerta()
        {
        }

        #endregion


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
                Label.Set(ctx, task.Result.Item1);
                Score.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string,double>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var text = Text.Get(context);

            SentimentAnalyzerDeBERTA analyzer = new SentimentAnalyzerDeBERTA(ProcessScope.GetHttpClient());

            var res=await analyzer.Analyze(text);

            return res;
        }

        #endregion
    }
}

