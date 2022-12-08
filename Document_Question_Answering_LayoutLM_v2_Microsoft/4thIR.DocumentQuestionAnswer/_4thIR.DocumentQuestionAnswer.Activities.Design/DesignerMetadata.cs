using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.DocumentQuestionAnswer.Activities.Design.Designers;
using _4thIR.DocumentQuestionAnswer.Activities.Design.Properties;

namespace _4thIR.DocumentQuestionAnswer.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(DocumentQuestionAnswering), categoryAttribute);
            builder.AddCustomAttributes(typeof(DocumentQuestionAnswering), new DesignerAttribute(typeof(DocumentQuestionAnsweringDesigner)));
            builder.AddCustomAttributes(typeof(DocumentQuestionAnswering), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
