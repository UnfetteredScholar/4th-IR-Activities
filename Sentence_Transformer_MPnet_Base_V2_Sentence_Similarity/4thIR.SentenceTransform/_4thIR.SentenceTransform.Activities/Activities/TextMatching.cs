using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.SentenceTransform.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using SentenceMatching.MPnetBase;
using SentenceMatching.MPnetBase.V2.SentenceSimilarity;

namespace _4thIR.SentenceTransform.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextMatching_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextMatching_Description))]
    public class TextMatching : ContinuableAsyncCodeActivity
    {
        private static readonly SentenceSimilarityAnalyzer _analyzer = new SentenceSimilarityAnalyzer();

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

        [LocalizedDisplayName(nameof(Resources.TextMatching_Sentences_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextMatching_Sentences_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> Sentences { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextMatching_MatchedSentences_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextMatching_MatchedSentences_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<MatchedSentence[]> MatchedSentences { get; set; }

        #endregion


        #region Constructors

        public TextMatching()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Sentences == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Sentences)));

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
                MatchedSentences.Set(ctx, task.Result);
            };
        }

        private async Task<MatchedSentence[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentences = Sentences.Get(context);
            var res = await _analyzer.MatchSentences(sentences);

            return res;
        }

        #endregion
    }
}

