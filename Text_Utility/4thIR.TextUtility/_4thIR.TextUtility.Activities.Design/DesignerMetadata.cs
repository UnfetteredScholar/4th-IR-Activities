using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextUtility.Activities.Design.Designers;
using _4thIR.TextUtility.Activities.Design.Properties;

namespace _4thIR.TextUtility.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextTokenizationNLP), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextTokenizationNLP), new DesignerAttribute(typeof(TextTokenizationNLPDesigner)));
            builder.AddCustomAttributes(typeof(TextTokenizationNLP), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(TextTokenizationSpacy), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextTokenizationSpacy), new DesignerAttribute(typeof(TextTokenizationSpacyDesigner)));
            builder.AddCustomAttributes(typeof(TextTokenizationSpacy), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ImageTextExtraction), categoryAttribute);
            builder.AddCustomAttributes(typeof(ImageTextExtraction), new DesignerAttribute(typeof(ImageTextExtractionDesigner)));
            builder.AddCustomAttributes(typeof(ImageTextExtraction), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(PDFTextExtraction), categoryAttribute);
            builder.AddCustomAttributes(typeof(PDFTextExtraction), new DesignerAttribute(typeof(PDFTextExtractionDesigner)));
            builder.AddCustomAttributes(typeof(PDFTextExtraction), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(PDFParsing), categoryAttribute);
            builder.AddCustomAttributes(typeof(PDFParsing), new DesignerAttribute(typeof(PDFParsingDesigner)));
            builder.AddCustomAttributes(typeof(PDFParsing), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
