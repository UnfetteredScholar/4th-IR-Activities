using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.SCompletePosT.Activities.Design.Designers;
using _4thIR.SCompletePosT.Activities.Design.Properties;

namespace _4thIR.SCompletePosT.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextCompletion), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextCompletion), new DesignerAttribute(typeof(TextCompletionDesigner)));
            builder.AddCustomAttributes(typeof(TextCompletion), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
