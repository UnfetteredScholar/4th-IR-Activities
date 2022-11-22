using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.NERecognition.Activities.Design.Designers;
using _4thIR.NERecognition.Activities.Design.Properties;

namespace _4thIR.NERecognition.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(NameEntityRecognition), categoryAttribute);
            builder.AddCustomAttributes(typeof(NameEntityRecognition), new DesignerAttribute(typeof(NameEntityRecognitionDesigner)));
            builder.AddCustomAttributes(typeof(NameEntityRecognition), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
