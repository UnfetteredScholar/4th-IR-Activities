using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextComparison.Activities.Design.Designers;
using _4thIR.TextComparison.Activities.Design.Properties;

namespace _4thIR.TextComparison.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextComparison), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextComparison), new DesignerAttribute(typeof(TextComparisonDesigner)));
            builder.AddCustomAttributes(typeof(TextComparison), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
