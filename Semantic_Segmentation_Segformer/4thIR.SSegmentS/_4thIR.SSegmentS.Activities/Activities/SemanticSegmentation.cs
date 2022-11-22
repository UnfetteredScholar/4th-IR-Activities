using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.SSegmentS.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using SemanticSegmentation.Segformer;

namespace _4thIR.SSegmentS.Activities
{
    [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_DisplayName))]
    [LocalizedDescription(nameof(Resources.SemanticSegmentation_Description))]
    public class SemanticSegmentation : ContinuableAsyncCodeActivity
    {
        private static readonly SemanticSegmentizerSegformer _segmentizer = new SemanticSegmentizerSegformer();

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

        [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmentation_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_SegmentationType_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmentation_SegmentationType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<SegmentationType> SegmentationType { get; set; }

        [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_StorageLocation_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmentation_StorageLocation_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> StorageLocation { get; set; } = "";

        #endregion


        #region Constructors

        public SemanticSegmentation()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));
            if (SegmentationType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SegmentationType)));

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
            var segmentationType = SegmentationType.Get(context);
            var storageLocation = StorageLocation.Get(context);
            await _segmentizer.SegmentImage(path, segmentationType, storageLocation);
        }

        #endregion
    }
}

