using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.AgeClassGVB.Activities.Design.Designers;
using _4thIR.AgeClassGVB.Activities.Design.Properties;

namespace _4thIR.AgeClassGVB.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(AgeClassification), categoryAttribute);
            builder.AddCustomAttributes(typeof(AgeClassification), new DesignerAttribute(typeof(AgeClassificationDesigner)));
            builder.AddCustomAttributes(typeof(AgeClassification), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
