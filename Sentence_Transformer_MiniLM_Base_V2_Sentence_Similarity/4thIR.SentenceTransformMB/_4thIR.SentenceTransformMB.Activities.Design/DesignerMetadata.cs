using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.SentenceTransformMB.Activities.Design.Designers;
using _4thIR.SentenceTransformMB.Activities.Design.Properties;

namespace _4thIR.SentenceTransformMB.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextMatching), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextMatching), new DesignerAttribute(typeof(TextMatchingDesigner)));
            builder.AddCustomAttributes(typeof(TextMatching), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
