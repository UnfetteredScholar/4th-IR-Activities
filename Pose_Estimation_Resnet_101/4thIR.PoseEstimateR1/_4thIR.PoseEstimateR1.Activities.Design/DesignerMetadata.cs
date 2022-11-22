using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.PoseEstimateR1.Activities.Design.Designers;
using _4thIR.PoseEstimateR1.Activities.Design.Properties;

namespace _4thIR.PoseEstimateR1.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(PoseEstimation), categoryAttribute);
            builder.AddCustomAttributes(typeof(PoseEstimation), new DesignerAttribute(typeof(PoseEstimationDesigner)));
            builder.AddCustomAttributes(typeof(PoseEstimation), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
