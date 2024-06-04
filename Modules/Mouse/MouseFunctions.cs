using System.Drawing;
using System.Runtime.InteropServices;

namespace NITHdmis.Modules.Mouse
{
    internal static class MouseFunctions
    {
        private static MousePoint lpPoint;

        public static Point GetCursorPosition()
        {
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            //if (!success)
            return lpPoint;
        }

        //This simulates a left mouse click
        // TODO deprecated
        private static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event((int)MouseButtonFlags.LeftDown, xpos, ypos, 0, 0);
            mouse_event((int)MouseButtonFlags.LeftUp, xpos, ypos, 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void MouseEvent(MouseButtonFlags value)
        {
            Point position = GetCursorPosition();

            mouse_event
                ((int)value,
                 position.X,
                 position.Y,
                 0,
                 0)
                ;
        }

        public static void WheelMove(int speed)
        {
            Point position = GetCursorPosition();

            mouse_event
                (0x0800,
                 position.X,
                 position.Y,
                 speed,
                 0)
                ;
        }

        [DllImport("user32")]
        private static extern int SetCursorPos(int x, int y);

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        [DllImport("user32")]
        private static extern int ShowCursor(bool bShow);

        public static void ShowMouseCursor(bool show)
        {
            ShowCursor(show);
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out MousePoint lpPoint);

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public static implicit operator Point(MousePoint point)
            {
                return new Point(point.X, point.Y);
            }
        }
    }
}