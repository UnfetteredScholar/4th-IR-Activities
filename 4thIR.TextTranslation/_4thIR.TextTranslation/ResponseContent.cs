using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextTranslation
{
    internal class ResponseContent
    {
        public string original_text { get; set; }
        public string conversion_text { get; set; }

        public ResponseContent()
        {
            original_text = null;
            conversion_text = null;
        }
    }
}
