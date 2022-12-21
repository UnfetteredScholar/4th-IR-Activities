
namespace PoseEstimation.mmPose
{
    /// <summary>
    /// Stores information about detected human figures
    /// </summary>
    public class DetectedFigure
    {
        public DetectedFigure()
        {
          
        }

        public double[][] Keypoints { get; set; }
        public double Score { get; set; }
        public double Area { get; set; }
    }
}
