using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.DocClassifyVLA.Activities.Design.Designers;
using _4thIR.DocClassifyVLA.Activities.Design.Properties;

namespace _4thIR.DocClassifyVLA.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(DocumentClassification), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentClassification), new DesignerAttribute(typeof(DocumentClassificationDesigner)));
            builder.AddCustomAttributes(typeof(DocumentClassification), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentMetaExtraction), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentMetaExtraction), new DesignerAttribute(typeof(DocumentMetaExtractionDesigner)));
            builder.AddCustomAttributes(typeof(DocumentMetaExtraction), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentAnalysis), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentAnalysis), new DesignerAttribute(typeof(DocumentAnalysisDesigner)));
            builder.AddCustomAttributes(typeof(DocumentAnalysis), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentTextAnalysis), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentTextAnalysis), new DesignerAttribute(typeof(DocumentTextAnalysisDesigner)));
            builder.AddCustomAttributes(typeof(DocumentTextAnalysis), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentTextExtraction), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentTextExtraction), new DesignerAttribute(typeof(DocumentTextExtractionDesigner)));
            builder.AddCustomAttributes(typeof(DocumentTextExtraction), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
