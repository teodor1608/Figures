using System.Drawing;

namespace Figures.ClassLibraryFigures
{
    public class TriangleExtensions
    {
        public static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        public static Point[] GeneratePoints(Point location, int width, int height)//Generates corner points for the triangle from the Location and Size
        {
            Point point2 = new Point(location.X + (width / 2), location.Y);
            Point point3 = new Point(location.X, location.Y + height);
            Point point1 = new Point(location.X + width, location.Y + height);
            Point[] PointsTemp =
            {
                point1,
                point2,
                point3
            };

            return PointsTemp;
        }

        public static float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

    }
}
