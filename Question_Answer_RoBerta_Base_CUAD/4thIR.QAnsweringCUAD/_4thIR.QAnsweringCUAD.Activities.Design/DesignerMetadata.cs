using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.QAnsweringCUAD.Activities.Design.Designers;
using _4thIR.QAnsweringCUAD.Activities.Design.Properties;

namespace _4thIR.QAnsweringCUAD.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(QuestionAnswering), categoryAttribute);
            builder.AddCustomAttributes(typeof(QuestionAnswering), new DesignerAttribute(typeof(QuestionAnsweringDesigner)));
            builder.AddCustomAttributes(typeof(QuestionAnswering), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
