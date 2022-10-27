using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4thIR.ObjectDetectY4.ObjectDetection.Exceptions
{
    public class ObjectDetectException : Exception
    {
        public ObjectDetectException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
