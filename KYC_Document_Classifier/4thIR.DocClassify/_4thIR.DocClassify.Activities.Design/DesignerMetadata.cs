using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.DocClassify.Activities.Design.Designers;
using _4thIR.DocClassify.Activities.Design.Properties;

namespace _4thIR.DocClassify.Activities.Design
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


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
