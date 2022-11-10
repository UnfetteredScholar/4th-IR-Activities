using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.PoseEstimateR1.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using PoseEstimation.Resnet101;

namespace _4thIR.PoseEstimateR1.Activities
{
    [LocalizedDisplayName(nameof(Resources.PoseEstimation_DisplayName))]
    [LocalizedDescription(nameof(Resources.PoseEstimation_Description))]
    public class PoseEstimation : ContinuableAsyncCodeActivity
    {
        private static readonly PoseEstimatorR101 _poseEstimator = new PoseEstimatorR101();

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

        [LocalizedDisplayName(nameof(Resources.PoseEstimation_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.PoseEstimation_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.PoseEstimation_StorageLocation_DisplayName))]
        [LocalizedDescription(nameof(Resources.PoseEstimation_StorageLocation_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> StorageLocation { get; set; } = "";

        #endregion


        #region Constructors

        public PoseEstimation()
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
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var storageLocation = StorageLocation.Get(context);
            await _poseEstimator.DetectFigures(path, storageLocation);
        }

        #endregion
    }
}

