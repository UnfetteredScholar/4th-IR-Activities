using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.SSegmentS.Activities.Design.Designers;
using _4thIR.SSegmentS.Activities.Design.Properties;

namespace _4thIR.SSegmentS.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SemanticSegmentation), categoryAttribute);
            builder.AddCustomAttributes(typeof(SemanticSegmentation), new DesignerAttribute(typeof(SemanticSegmentationDesigner)));
            builder.AddCustomAttributes(typeof(SemanticSegmentation), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
