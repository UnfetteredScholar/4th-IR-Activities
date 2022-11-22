using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.ImageClassifyGVB.Activities.Design.Designers;
using _4thIR.ImageClassifyGVB.Activities.Design.Properties;

namespace _4thIR.ImageClassifyGVB.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ImageClassification), categoryAttribute);
            builder.AddCustomAttributes(typeof(ImageClassification), new DesignerAttribute(typeof(ImageClassificationDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassification), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
