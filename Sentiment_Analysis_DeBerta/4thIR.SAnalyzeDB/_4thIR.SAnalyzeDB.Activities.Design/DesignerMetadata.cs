using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.SAnalyzeDB.Activities.Design.Designers;
using _4thIR.SAnalyzeDB.Activities.Design.Properties;

namespace _4thIR.SAnalyzeDB.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SentimentAnalysis), categoryAttribute);
            builder.AddCustomAttributes(typeof(SentimentAnalysis), new DesignerAttribute(typeof(SentimentAnalysisDesigner)));
            builder.AddCustomAttributes(typeof(SentimentAnalysis), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
