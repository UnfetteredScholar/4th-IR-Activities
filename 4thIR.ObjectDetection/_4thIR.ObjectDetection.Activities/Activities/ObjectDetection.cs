using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.ObjectDetection.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using EfficientDetObjectDetection;

namespace _4thIR.ObjectDetection.Activities
{
    [LocalizedDisplayName(nameof(Resources.ObjectDetection_DisplayName))]
    [LocalizedDescription(nameof(Resources.ObjectDetection_Description))]
    public class ObjectDetection : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ObjectDetection_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetection_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.ObjectDetection_Detections_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetection_Detections_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<int> Detections { get; set; }

        [LocalizedDisplayName(nameof(Resources.ObjectDetection_DetectionBoxes_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetection_DetectionBoxes_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double[,]> DetectionBoxes { get; set; }

        [LocalizedDisplayName(nameof(Resources.ObjectDetection_DetectionClasses_DisplayName))]
        [LocalizedDescription(nameof(Resources.ObjectDetection_DetectionClasses_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double[]> DetectionClasses { get; set; }

        #endregion


        #region Constructors

        public ObjectDetection()
        {
        }

        #endregion

        private readonly ObjectDetector objectDetector = new ObjectDetector();

        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var path = Path.Get(context);

            var result = await objectDetector.DetectObject(path);

            // Outputs
            return (ctx) => {
                Detections.Set(ctx, result.Item1);
                DetectionBoxes.Set(ctx, result.Item2);
                DetectionClasses.Set(ctx, result.Item3);
            };
        }

        #endregion
    }
}

