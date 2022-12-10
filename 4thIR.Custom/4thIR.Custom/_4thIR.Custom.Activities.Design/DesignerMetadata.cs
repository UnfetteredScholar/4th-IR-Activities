using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.Custom.Activities.Design.Designers;
using _4thIR.Custom.Activities.Design.Properties;

namespace _4thIR.Custom.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            #region Process Scope
            builder.AddCustomAttributes(typeof(ProcessScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(ProcessScope), new DesignerAttribute(typeof(ProcessScopeDesigner)));
            builder.AddCustomAttributes(typeof(ProcessScope), new HelpKeywordAttribute(""));
            #endregion


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
