using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.AgeClassGVB.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using AgeClassification.GoogeVitBase;

namespace _4thIR.AgeClassGVB.Activities
{
    [LocalizedDisplayName(nameof(Resources.AgeClassification_DisplayName))]
    [LocalizedDescription(nameof(Resources.AgeClassification_Description))]
    public class AgeClassification : ContinuableAsyncCodeActivity
    {
        private static readonly AgeClassifierGVB _classifier = new AgeClassifierGVB();

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

        [LocalizedDisplayName(nameof(Resources.AgeClassification_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.AgeClassification_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.AgeClassification_AgeRanges_DisplayName))]
        [LocalizedDescription(nameof(Resources.AgeClassification_AgeRanges_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<AgeProbabilityPair[]> AgeRanges { get; set; }

        #endregion


        #region Constructors

        public AgeClassification()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));

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
                AgeRanges.Set(ctx, task.Result);
            };
        }

        private async Task<AgeProbabilityPair[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var res = await _classifier.ClassifyAge(path);

            return res;
        }

        #endregion
    }
}

