using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using TextGeneration.Fairseq;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextGenerationFairseq_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextGenerationFairseq_Description))]
    public class TextGenerationFairseq : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextGenerationFairseq_Sentence_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGenerationFairseq_Sentence_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Sentence { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextGenerationFairseq_GeneratedText1_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGenerationFairseq_GeneratedText1_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> GeneratedText1 { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextGenerationFairseq_GeneratedText2_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextGenerationFairseq_GeneratedText2_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> GeneratedText2 { get; set; }

        #endregion


        #region Constructors

        public TextGenerationFairseq()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentence == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentence)));

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
                GeneratedText1.Set(ctx, task.Result.Item1);
                GeneratedText2.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string,string>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentence = Sentence.Get(context);

            TextGeneratorFairseq generator = new TextGeneratorFairseq(ProcessScope.GetHttpClient());

            var result = await generator.GenerateText(sentence);

            return result;
        }

        #endregion
    }
}

