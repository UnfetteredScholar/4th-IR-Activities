using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using classifier.Activities.Design.Designers;
using classifier.Activities.Design.Properties;

namespace classifier.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ImageClassifier), categoryAttribute);
            builder.AddCustomAttributes(typeof(ImageClassifier), new DesignerAttribute(typeof(ImageClassifierDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassifier), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
