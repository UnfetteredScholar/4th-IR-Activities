using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.PoseEstimateSH.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using PoseEstimation.OpenVino;

namespace _4thIR.PoseEstimateSH.Activities
{
    [LocalizedDisplayName(nameof(Resources.PoseEstimation_DisplayName))]
    [LocalizedDescription(nameof(Resources.PoseEstimation_Description))]
    public class PoseEstimation : ContinuableAsyncCodeActivity
    {
        private static readonly PoseEstimator _poseEstimator = new PoseEstimator();
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

        [LocalizedDisplayName(nameof(Resources.PoseEstimation_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.PoseEstimation_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.PoseEstimation_Threshold_DisplayName))]
        [LocalizedDescription(nameof(Resources.PoseEstimation_Threshold_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<double> Threshold { get; set; } = 0.5;

        [LocalizedDisplayName(nameof(Resources.PoseEstimation_OutputFolder_DisplayName))]
        [LocalizedDescription(nameof(Resources.PoseEstimation_OutputFolder_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> OutputFolder { get; set; } = "";

        #endregion


        #region Constructors

        public PoseEstimation()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));

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
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var filePath = FilePath.Get(context);
            var threshold = Threshold.Get(context);
            var outputFolder = OutputFolder.Get(context);
            await _poseEstimator.DetectFigures(filePath, threshold, outputFolder);
        }

        #endregion
    }
}

