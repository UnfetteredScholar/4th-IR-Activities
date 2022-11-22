using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextSummarizeF.Activities.Design.Designers;
using _4thIR.TextSummarizeF.Activities.Design.Properties;

namespace _4thIR.TextSummarizeF.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextSummarization), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextSummarization), new DesignerAttribute(typeof(TextSummarizationDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarization), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
