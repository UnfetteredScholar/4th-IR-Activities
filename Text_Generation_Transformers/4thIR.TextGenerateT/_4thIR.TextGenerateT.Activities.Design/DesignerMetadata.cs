using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextGenerateT.Activities.Design.Designers;
using _4thIR.TextGenerateT.Activities.Design.Properties;

namespace _4thIR.TextGenerateT.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextGeneration), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextGeneration), new DesignerAttribute(typeof(TextGenerationDesigner)));
            builder.AddCustomAttributes(typeof(TextGeneration), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
