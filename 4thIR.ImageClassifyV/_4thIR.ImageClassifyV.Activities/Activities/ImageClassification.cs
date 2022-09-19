using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.ImageClassifyV.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using ImageClassification;

namespace _4thIR.ImageClassifyV.Activities
{
    [LocalizedDisplayName(nameof(Resources.ImageClassification_DisplayName))]
    [LocalizedDescription(nameof(Resources.ImageClassification_Description))]
    public class ImageClassification : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassification_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassification_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassification_Label_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassification_Label_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Label { get; set; }

        #endregion


        #region Constructors

        public ImageClassification()
        {
        }

        #endregion
        private readonly ImageClassifier classifier = new ImageClassifier();

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

            var result = await classifier.ClassifyImage(path);

            // Outputs
            return (ctx) => {
                Label.Set(ctx, result);
            };
        }

        #endregion
    }
}

