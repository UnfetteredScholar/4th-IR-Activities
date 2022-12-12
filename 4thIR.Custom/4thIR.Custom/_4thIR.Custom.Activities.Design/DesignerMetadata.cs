using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.Custom.Activities.Design.Designers;
using _4thIR.Custom.Activities.Design.Properties;

namespace _4thIR.Custom.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            #region Process Scope
            builder.AddCustomAttributes(typeof(ProcessScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(ProcessScope), new DesignerAttribute(typeof(ProcessScopeDesigner)));
            builder.AddCustomAttributes(typeof(ProcessScope), new HelpKeywordAttribute(""));
            #endregion

            #region Document Classification KYC
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new CategoryAttribute("4th-IR.Document Classification.KYC"));
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new DesignerAttribute(typeof(DocumentClassificationKYCDesigner)));
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new HelpKeywordAttribute(""));
            #endregion

            #region Text Translation - Transformers (Multilanguage)
            builder.AddCustomAttributes(typeof(TextTranslationTM), new CategoryAttribute("4th-IR.TextTranslation.Multilanguage"));
            builder.AddCustomAttributes(typeof(TextTranslationTM), new DesignerAttribute(typeof(TextTranslationTMDesigner)));
            builder.AddCustomAttributes(typeof(TextTranslationTM), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Vissl (RegnetY-60)
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new CategoryAttribute("4th-IR.Image Classification.Vissl"));
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new DesignerAttribute(typeof(ImageClassificationVisslDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new HelpKeywordAttribute(""));
            #endregion

            #region ObjectDetection.EfficientDet
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new CategoryAttribute("4th-IR.Object Detection.EfficientDet"));
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new DesignerAttribute(typeof(ObjectDetectionEDDesigner)));
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Classy Vision
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new CategoryAttribute("4th-IR.Image Classification.ClassyVision"));
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new DesignerAttribute(typeof(ImageClassificationCVDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Resnet 50
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new CategoryAttribute("4th-IR.Image Classification.Resnet 50"));
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new DesignerAttribute(typeof(ImageClassificationR50Designer)));
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new HelpKeywordAttribute(""));
            #endregion

            #region Roberta Base Squad 2 - Question Answer
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new CategoryAttribute("4th-IR.Question Answering.Roberta Base Squad 2"));
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new DesignerAttribute(typeof(QuestionAnsweringRBS2Designer)));
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - BigBird
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new CategoryAttribute("4th-IR.Text Summarization.Big Bird"));
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new DesignerAttribute(typeof(TextSummarizationBBDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new HelpKeywordAttribute(""));
            #endregion

            #region Part of Speech - Flair
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new CategoryAttribute("4th-IR.Part of Speech.Flair"));
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new DesignerAttribute(typeof(PartOfSpeechIdentificationFlairDesigner)));
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new HelpKeywordAttribute(""));
            #endregion

            #region Name Entity Recognition - Flair
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new CategoryAttribute("4th-IR.Name Entity Recognition.Flair"));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new DesignerAttribute(typeof(NameEntityRecognitionFlairDesigner)));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new HelpKeywordAttribute(""));
            #endregion

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
