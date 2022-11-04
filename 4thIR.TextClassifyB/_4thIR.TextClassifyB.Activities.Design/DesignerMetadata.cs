using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextClassifyB.Activities.Design.Designers;
using _4thIR.TextClassifyB.Activities.Design.Properties;

namespace _4thIR.TextClassifyB.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextClassification), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextClassification), new DesignerAttribute(typeof(TextClassificationDesigner)));
            builder.AddCustomAttributes(typeof(TextClassification), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
