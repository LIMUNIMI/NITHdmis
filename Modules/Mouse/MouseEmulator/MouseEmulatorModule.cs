using System.Drawing;
using NITHlibrary.Tools.Filters.PointFilters;

namespace NITHdmis.Modules.Mouse.MouseEmulator
{
    public class MouseEmulatorModule
    {
        private IPointFilter filter;
        private Point currentInput = new Point();
        private bool enabled = false;
        private bool cursorVisible = true;

        public MouseEmulatorModule(IPointFilter filter)
        {
            Filter = filter;
        }

        public IPointFilter Filter { get => filter; set => filter = value; }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool CursorVisible
        {
            get { return cursorVisible; }
            set
            {
                cursorVisible = value;
                MouseFunctions.ShowMouseCursor(cursorVisible);
                
            }
        }

        public void SetCursorCoordinates(double X, double Y)
        {
            if (enabled)
            {
                currentInput.X = (int)X;
                currentInput.Y = (int)Y;

                Filter.Push(currentInput);

                MouseFunctions.SetCursorPosition(Filter.GetOutput().X, Filter.GetOutput().Y);
            }
        }
    }
}