using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.HGenerateTS.Activities.Design.Designers;
using _4thIR.HGenerateTS.Activities.Design.Properties;

namespace _4thIR.HGenerateTS.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(HeadlineGeneration), categoryAttribute);
            builder.AddCustomAttributes(typeof(HeadlineGeneration), new DesignerAttribute(typeof(HeadlineGenerationDesigner)));
            builder.AddCustomAttributes(typeof(HeadlineGeneration), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
