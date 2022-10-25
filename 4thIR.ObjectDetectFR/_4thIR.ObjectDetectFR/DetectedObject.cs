﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectDetectionFRC
{
    /// <summary>
    /// Stores information about detected objects
    /// </summary>
    public class DetectedObject
    {
        /// <summary>
        /// Creates an instance of the DetectedObject class
        /// </summary>
        public DetectedObject()
        {

        }

        public double[][] boxes { get; set; }
        public double[] labels { get; set; }
        public double[] scores { get; set; }
    }
}
