using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.DocClassify.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using DocumentClassification;

namespace _4thIR.DocClassify.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentClassification_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentClassification_Description))]
    public class DocumentClassification : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_DocumentType_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_DocumentType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<DocumentType> DocumentType { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentClassification_DocumentClass_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentClassification_DocumentClass_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> DocumentClass { get; set; }

        #endregion

        private readonly DocumentClassifier classifier = new DocumentClassifier();

        #region Constructors

        public DocumentClassification()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));
            if (DocumentType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DocumentType)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var filePath = FilePath.Get(context);
            var documentType = DocumentType.Get(context);

            string documentContent = classifier.ReadDocumentContentBase64(filePath);
            string result =await classifier.ClassifyDocument(documentContent, documentType);

            // Outputs
            return (ctx) => {
                DocumentClass.Set(ctx, result);
            };
        }

        #endregion
    }
}

