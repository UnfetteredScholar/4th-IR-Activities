using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.SentenceTransformMB.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using SentenceMatching.SentenceSimilarity.Minilm;
using _4thIR.ProcessScope.Activities.Activities;

namespace _4thIR.SentenceTransformMB.Activities
{
    [LocalizedDisplayName(nameof(Resources.TextMatching_DisplayName))]
    [LocalizedDescription(nameof(Resources.TextMatching_Description))]
    public class TextMatching : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.TextMatching_SentencesA_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextMatching_SentencesA_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> SentencesA { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextMatching_SentencesB_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextMatching_SentencesB_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> SentencesB { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextMatching_SimilarityScores_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextMatching_SimilarityScores_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<SimilarityScore[]> SimilarityScores { get; set; }

        #endregion


        #region Constructors

        public TextMatching()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SentencesA == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SentencesA)));
            if (SentencesB == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SentencesB)));

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
                SimilarityScores.Set(ctx, task.Result);
            };
        }

        private async Task<SimilarityScore[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var sentencesA = SentencesA.Get(context);
            var sentencesB = SentencesB.Get(context);

            TextSimilarityComparer sComparer = new TextSimilarityComparer(ProcessScope.Activities.ProcessScope.GetHttpClient());

            var res = await sComparer.CompareSentences(sentencesA, sentencesB);


            return res;
        }

        #endregion
    }
}

