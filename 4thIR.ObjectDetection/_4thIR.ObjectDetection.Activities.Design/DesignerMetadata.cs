using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.ObjectDetection.Activities.Design.Designers;
using _4thIR.ObjectDetection.Activities.Design.Properties;

namespace _4thIR.ObjectDetection.Activities.Design
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

            builder.AddCustomAttributes(typeof(ImageClassification), categoryAttribute);
            builder.AddCustomAttributes(typeof(ImageClassification), new DesignerAttribute(typeof(ImageClassificationDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassification), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
