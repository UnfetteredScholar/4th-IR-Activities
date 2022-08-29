using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextTranslation.Activities.Design.Designers;
using _4thIR.TextTranslation.Activities.Design.Properties;

namespace _4thIR.TextTranslation.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(Translation), categoryAttribute);
            builder.AddCustomAttributes(typeof(Translation), new DesignerAttribute(typeof(TranslationDesigner)));
            builder.AddCustomAttributes(typeof(Translation), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
