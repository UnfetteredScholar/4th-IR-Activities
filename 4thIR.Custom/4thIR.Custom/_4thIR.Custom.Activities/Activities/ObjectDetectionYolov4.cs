using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using ObjectDetection.Yolov4;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.ObjectDetectionYolov4_DisplayName))]
    [LocalizedDescription(nameof(Resources.ObjectDetectionYolov4_Description))]
    public class ObjectDetectionYolov4 : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.ObjectDetectionYolov4_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetectionYolov4_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.ObjectDetectionYolov4_StorageLocation_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetectionYolov4_StorageLocation_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> StorageLocation { get; set; } = "";

        #endregion


        #region Constructors

        public ObjectDetectionYolov4()
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

            ObjectDetectorY4 detector = new ObjectDetectorY4(ProcessScope.GetHttpClient());

            await detector.DetectObject(path, storageLocation);
        }

        #endregion
    }
}

