using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.HwrittenRecOV.Activities.Design.Designers;
using _4thIR.HwrittenRecOV.Activities.Design.Properties;

namespace _4thIR.HwrittenRecOV.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(HandwrittenRecognition), categoryAttribute);
            builder.AddCustomAttributes(typeof(HandwrittenRecognition), new DesignerAttribute(typeof(HandwrittenRecognitionDesigner)));
            builder.AddCustomAttributes(typeof(HandwrittenRecognition), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
