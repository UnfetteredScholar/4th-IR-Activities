using System;

namespace EfficientDetObjectDetection
{
    internal class ResponseContent
    {
        public int num_detections { get; set; }
        public double[,] detection_boxes { get; set; }
        public double[] detection_classes { get; set; }
    }
}
