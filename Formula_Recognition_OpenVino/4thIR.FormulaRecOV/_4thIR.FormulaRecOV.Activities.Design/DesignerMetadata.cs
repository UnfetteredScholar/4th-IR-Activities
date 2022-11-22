using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.FormulaRecOV.Activities.Design.Designers;
using _4thIR.FormulaRecOV.Activities.Design.Properties;

namespace _4thIR.FormulaRecOV.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(FormulaRecognition), categoryAttribute);
            builder.AddCustomAttributes(typeof(FormulaRecognition), new DesignerAttribute(typeof(FormulaRecognitionDesigner)));
            builder.AddCustomAttributes(typeof(FormulaRecognition), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
