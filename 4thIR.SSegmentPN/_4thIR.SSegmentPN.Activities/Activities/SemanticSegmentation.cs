using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.SSegmentPN.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using SemanticSegmentation.PSPNet;

namespace _4thIR.SSegmentPN.Activities
{
    [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_DisplayName))]
    [LocalizedDescription(nameof(Resources.SemanticSegmentation_Description))]
    public class SemanticSegmentation : ContinuableAsyncCodeActivity
    {
        private static readonly SemanticSegmentizerPSPNet _semanticSegmentizer = new SemanticSegmentizerPSPNet();

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

        [LocalizedDisplayName(nameof(Resources.SemanticSegmentation_Label_DisplayName))]
        [LocalizedDescription(nameof(Resources.SemanticSegmentation_Label_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Label { get; set; } = "";

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
            var label = Label.Get(context);
            await _semanticSegmentizer.SegmentImage(path, label);
        }

        #endregion
    }
}

