using System.Drawing;

namespace NITHdmis.Eyetracking.PointFilters
{
    /// <summary>
    /// A filter which does... Nothing! Output = input.
    /// </summary>
    public class PointFilterBypass : IPointFilter
    {
        private Point point;

        public PointFilterBypass()
        {
            this.point = new Point(0,0);
        }

        public void Push(Point point)
        {
            this.point = point;
        }

        public Point GetOutput()
        {
            return point;
        }
    }
}
