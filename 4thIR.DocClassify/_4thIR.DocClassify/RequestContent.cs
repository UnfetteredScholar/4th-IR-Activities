using System;

namespace DocumentClassification
{
    internal class RequestContent
    {
        public string document_content { get; set; }
        public string document_type { get; set; }

        public RequestContent()
        {

        }

    }
}
