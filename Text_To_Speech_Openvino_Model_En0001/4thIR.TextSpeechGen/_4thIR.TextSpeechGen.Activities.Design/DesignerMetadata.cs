using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextSpeechGen.Activities.Design.Designers;
using _4thIR.TextSpeechGen.Activities.Design.Properties;

namespace _4thIR.TextSpeechGen.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SpeechGeneration), categoryAttribute);
            builder.AddCustomAttributes(typeof(SpeechGeneration), new DesignerAttribute(typeof(SpeechGenerationDesigner)));
            builder.AddCustomAttributes(typeof(SpeechGeneration), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
