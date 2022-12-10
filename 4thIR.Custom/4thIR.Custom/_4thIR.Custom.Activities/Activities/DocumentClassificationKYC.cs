using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using DocumentClassification.KYC;
using System.ComponentModel;
using UiPath.Shared.Activities.Utilities;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentClassificationKYC_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentClassificationKYC_Description))]
    public class DocumentClassificationKYC : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.DocumentClassificationKYC_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassificationKYC_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassificationKYC_DocumentType_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassificationKYC_DocumentType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<DocumentType>))]
        public DocumentType DocumentType { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassificationKYC_DocumentClass_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassificationKYC_DocumentClass_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> DocumentClass { get; set; }

        #endregion


        #region Constructors

        public DocumentClassificationKYC()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));
            if (DocumentType == 0) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DocumentType)));

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
                DocumentClass.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            DocumentClassifier classifier = new DocumentClassifier(ProcessScope.GetHttpClient());

            var filePath = FilePath.Get(context);

            var res = await classifier.ClassifyDocument(filePath, DocumentType);


            return res;
        }

        #endregion
    }
}

