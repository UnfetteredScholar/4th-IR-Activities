using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.PartSpeechIdentify.Activities.Design.Designers;
using _4thIR.PartSpeechIdentify.Activities.Design.Properties;

namespace _4thIR.PartSpeechIdentify.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(PartOfSpeechIdentification), categoryAttribute);
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentification), new DesignerAttribute(typeof(PartOfSpeechIdentificationDesigner)));
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentification), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
