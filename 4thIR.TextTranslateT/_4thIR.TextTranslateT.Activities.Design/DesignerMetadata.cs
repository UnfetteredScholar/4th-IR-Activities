using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.TextTranslateT.Activities.Design.Designers;
using _4thIR.TextTranslateT.Activities.Design.Properties;

namespace _4thIR.TextTranslateT.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TextTranslation), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextTranslation), new DesignerAttribute(typeof(TextTranslationDesigner)));
            builder.AddCustomAttributes(typeof(TextTranslation), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
