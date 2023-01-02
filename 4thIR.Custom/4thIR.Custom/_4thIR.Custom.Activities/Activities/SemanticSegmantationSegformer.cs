using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using System.ComponentModel;
using SemanticSegmentation.Segformer;
using UiPath.Shared.Activities.Utilities;

//class name typo, change in resources

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.SemanticSegmantationSegformer_DisplayName))]
    [LocalizedDescription(nameof(Resources.SemanticSegmantationSegformer_Description))]
    public class SemanticSegmantationSegformer : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.SemanticSegmantationSegformer_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmantationSegformer_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.SemanticSegmantationSegformer_StorageLocation_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmantationSegformer_StorageLocation_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> StorageLocation { get; set; }

        [LocalizedDisplayName(nameof(Resources.SemanticSegmantationSegformer_SegmantationType_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmantationSegformer_SegmantationType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<SegmentationType>))]
        public SegmentationType SegmentationType { get; set; }

        #endregion


        #region Constructors

        public SemanticSegmantationSegformer()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));
            if (SegmentationType == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SegmentationType)));

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

            SemanticSegmentizerSegformer semanticSegmentizer = new SemanticSegmentizerSegformer(ProcessScope.GetHttpClient());

            await semanticSegmentizer.SegmentImage(path, SegmentationType, storageLocation);
        }

        #endregion
    }
}

