
namespace PoseEstimation.mmPose
{
    /// <summary>
    /// Stores information about detected human figures
    /// </summary>
    public class DetectedFigure
    {
        public DetectedFigure(double[][] keypoints, double score, double area)
        {
            Keypoints = keypoints;
            Score = score;
            Area = area;

        }

        public double[][] Keypoints { get; set; }
        public double Score { get; set; }
        public double Area { get; set; }
    }
}
