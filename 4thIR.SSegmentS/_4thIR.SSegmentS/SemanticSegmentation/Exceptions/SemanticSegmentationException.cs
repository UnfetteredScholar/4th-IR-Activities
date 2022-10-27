using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4thIR.SSegmentS.SemanticSegmentation.Exceptions
{
    public class SemanticSegmentationException : Exception
    {
        public SemanticSegmentationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
