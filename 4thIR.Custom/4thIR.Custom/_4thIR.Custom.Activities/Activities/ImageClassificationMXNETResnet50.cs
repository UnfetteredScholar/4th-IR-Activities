using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using ImageClassification.MXNET.Resnet50;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.ImageClassificationMXNETResnet50_DisplayName))]
    [LocalizedDescription(nameof(Resources.ImageClassificationMXNETResnet50_Description))]
    public class ImageClassificationMXNETResnet50 : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.ImageClassificationMXNETResnet50_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassificationMXNETResnet50_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.ImageClassificationMXNETResnet50_Labels_DisplayName))]
        [LocalizedDescription(nameof(Resources.ImageClassificationMXNETResnet50_Labels_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<ClassificationLabel[]> Labels { get; set; }

        #endregion


        #region Constructors

        public ImageClassificationMXNETResnet50()
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
                Labels.Set(ctx, task.Result);
            };
        }

        private async Task<ClassificationLabel[]> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);

            ImageClassifierMXNET classifier = new ImageClassifierMXNET(ProcessScope.GetHttpClient());

            var res=await classifier.ClassifyImage(path);

            return res;
        }

        #endregion
    }
}

