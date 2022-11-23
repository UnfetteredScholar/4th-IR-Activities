using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.DGenerate.Activities.Design.Designers;
using _4thIR.DGenerate.Activities.Design.Properties;

namespace _4thIR.DGenerate.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(DialogueGeneration), categoryAttribute);
            builder.AddCustomAttributes(typeof(DialogueGeneration), new DesignerAttribute(typeof(DialogueGenerationDesigner)));
            builder.AddCustomAttributes(typeof(DialogueGeneration), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
