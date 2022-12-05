using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using _4thIR.DocClassify.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.ProcessScope.Activities.Activities;
using DocumentClassification.KYC;
using _4thIR.ProcessScope.Activities;
using UiPath.Shared.Activities.Utilities;

namespace _4thIR.DocClassify.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentClassification_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentClassification_Description))]
    public class DocumentClassification : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_DocumentType_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_DocumentType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [TypeConverter(typeof(EnumNameConverter<DocumentType>))]
        public DocumentType DocumentType { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_DocumentClass_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_DocumentClass_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> DocumentClass { get; set; }

        #endregion


        #region Constructors

        public DocumentClassification()
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
            DocumentClassifier classifier = new DocumentClassifier(ProcessScope.Activities.ProcessScope.GetHttpClient());
            
            var filePath = FilePath.Get(context);

            var res = await classifier.ClassifyDocument(filePath, DocumentType);


            return res;
        }

        #endregion
    }
}

