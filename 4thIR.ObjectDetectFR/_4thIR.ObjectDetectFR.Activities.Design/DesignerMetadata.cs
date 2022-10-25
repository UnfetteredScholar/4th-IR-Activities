using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.ObjectDetectFR.Activities.Design.Designers;
using _4thIR.ObjectDetectFR.Activities.Design.Properties;

namespace _4thIR.ObjectDetectFR.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ObjectDetection), categoryAttribute);
            builder.AddCustomAttributes(typeof(ObjectDetection), new DesignerAttribute(typeof(ObjectDetectionDesigner)));
            builder.AddCustomAttributes(typeof(ObjectDetection), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
