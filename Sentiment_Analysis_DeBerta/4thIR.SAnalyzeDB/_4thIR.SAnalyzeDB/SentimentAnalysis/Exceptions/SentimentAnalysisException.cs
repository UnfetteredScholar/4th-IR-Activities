using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysis.Exceptions
{
    public class SentimentAnalysisException:Exception
    {
        public SentimentAnalysisException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
