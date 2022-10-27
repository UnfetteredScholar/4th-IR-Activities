using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4thIR.PoseEstimateP.PoseEstimation.Exceptions
{
    public class PoseEstimateException : Exception
    {
        public PoseEstimateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
