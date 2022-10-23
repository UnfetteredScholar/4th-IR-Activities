using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.ImageClassifyFL.Activities.Design.Designers;
using _4thIR.ImageClassifyFL.Activities.Design.Properties;

namespace _4thIR.ImageClassifyFL.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ImageClassify), categoryAttribute);
            builder.AddCustomAttributes(typeof(ImageClassify), new DesignerAttribute(typeof(ImageClassifyDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassify), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
