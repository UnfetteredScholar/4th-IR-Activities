using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentClassification.Exceptions
{
    public class DocumentClassificationException:Exception
    {
        public DocumentClassificationException(string message, Exception innerException):base(message,innerException)
        {

        }
    }
}
