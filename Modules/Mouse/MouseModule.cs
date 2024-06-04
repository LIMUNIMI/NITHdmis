using System.Drawing;
using NITHlibrary.Tools.Filters.ValueFilters;
using NITHlibrary.Tools.Mappers;
using NITHlibrary.Tools.Timers;

namespace NITHdmis.Modules.Mouse
{
    public class MouseModule : IDisposable
    {
        public int FpsOffsetX = 500;
        public int FpsOffsetY = 500;
        public MouseModuleModes MouseMode = MouseModuleModes.Normal;
        public double MaxVelocityValue { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }
        public IDoubleFilter VelocityFilterX { get; set; }
        public IDoubleFilter VelocityFilterY { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        private double PastPositionX { get; set; }
        private double PastPositionY { get; set; }
        private VelocityExtractor1D VelocityExtractorX { get; set; } = new VelocityExtractor1D();
        private VelocityExtractor1D VelocityExtractorY { get; set; } = new VelocityExtractor1D();
        private MicroTimer PollingTimer { get; set; }
        public List<IMouseBehavior> Behaviors { get; set; } = new List<IMouseBehavior>();
        private MouseModuleSample Sample { get; set; }
        public float MultiplyFactor { get; set; }

        public MouseModule(long pollingMicroseconds = 10000, float multiplyFactor = 1f, MouseModuleModes MouseMode = MouseModuleModes.Normal, double MaxVelocityValue = 100000f)
        {
            this.VelocityFilterX = VelocityFilterX;
            this.VelocityFilterY = VelocityFilterY;
            this.PollingTimer = new MicroTimer(pollingMicroseconds);
            this.PollingTimer.MicroTimerElapsed += PollingTimer_MicroTimerElapsed;
            this.MultiplyFactor = multiplyFactor;
            this.MaxVelocityValue = MaxVelocityValue;
            this.MouseMode = MouseMode;

            // Initialize filters to bypass
            VelocityFilterX = new DoubleFilterBypass();
            VelocityFilterY = new DoubleFilterBypass();
        }

        public void StartPolling()
        {
            PollingTimer.Start();
        }

        public void StopPolling()
        {
            PollingTimer.Stop();
        }

        public Point GetInstantCursorPosition()
        {
            return MouseFunctions.GetCursorPosition();
        }

        public void SendMouseButtonEvent(MouseButtonFlags eventFlag)
        {
            MouseFunctions.MouseEvent(eventFlag);
        }

        public void SendMouseWheelMove(int speed)
        {
            MouseFunctions.WheelMove(speed);
        }

        public void SetCursorPosition(Point coordinates)
        {
            MouseFunctions.SetCursorPosition(coordinates.X, coordinates.Y);
        }

        public void SetCursorVisible(bool visible)
        {
            MouseFunctions.ShowMouseCursor(visible);
        }

        public void SetFpsOffsetToCurrentMousePosition()
        {
            FpsOffsetX = MouseFunctions.GetCursorPosition().X;
            FpsOffsetY = MouseFunctions.GetCursorPosition().Y;
        }

        private void PollingTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs e)
        {
            switch (MouseMode)
            {
                case MouseModuleModes.Normal:   // Mouse is free, but subsceptible to screen borders
                    PastPositionX = PositionX;
                    PastPositionY = PositionY;

                    Point pos = MouseFunctions.GetCursorPosition();
                    PositionX = pos.X;
                    PositionY = pos.Y;

                    VelocityExtractorX.Push(PositionX);
                    VelocityExtractorY.Push(PositionY);

                    double TempVelX = VelocityExtractorX.InstantSpeed * MultiplyFactor;
                    double TempVelY = VelocityExtractorY.InstantSpeed * MultiplyFactor;

                    // Check if velocity is more than max
                    if (TempVelX > MaxVelocityValue)
                    {
                        TempVelX = MaxVelocityValue;
                    }
                    if (TempVelY > MaxVelocityValue)
                    {
                        TempVelY = MaxVelocityValue;
                    }

                    VelocityFilterX.Push(TempVelX);
                    VelocityFilterY.Push(TempVelY);

                    VelocityX = VelocityFilterX.Pull();
                    VelocityY = VelocityFilterY.Pull();

                    DirectionX = VelocityExtractorX.Direction;
                    DirectionY = VelocityExtractorY.Direction;

                    Sample = new MouseModuleSample(VelocityX, VelocityY, PositionX, PositionY, DirectionX, DirectionY);

                    foreach (IMouseBehavior behavior in Behaviors)
                    {
                        behavior.ReceiveSample(Sample);
                    }
                    return;
                case MouseModuleModes.FPS:  // Mouse returns on center every time

                    Point pnt = MouseFunctions.GetCursorPosition();
                    PositionX = pnt.X - FpsOffsetX;
                    PositionY = pnt.Y - FpsOffsetY;

                    DirectionX = Math.Sign(PositionX);
                    DirectionY = Math.Sign(PositionY);

                    double TempVelX2 = Math.Abs(PositionX * MultiplyFactor);
                    double TempVelY2 = Math.Abs(PositionY * MultiplyFactor);

                    // Check if velocity is more than max
                    if (TempVelX2 > MaxVelocityValue)
                    {
                        TempVelX2 = MaxVelocityValue;
                    }
                    if (TempVelY2 > MaxVelocityValue)
                    {
                        TempVelY2 = MaxVelocityValue;
                    }

                    VelocityFilterX.Push(TempVelX2);
                    VelocityFilterY.Push(TempVelY2);

                    VelocityX = VelocityFilterX.Pull();
                    VelocityY = VelocityFilterY.Pull();

                    SetCursorPosition(new Point(FpsOffsetX, FpsOffsetY));     // Return pos to zero

                    Sample = new MouseModuleSample(VelocityX, VelocityY, PositionX, PositionY, DirectionX, DirectionY);

                    foreach (IMouseBehavior behavior in Behaviors)
                    {
                        behavior.ReceiveSample(Sample);
                    }

                    return;
            }
        }

        public void Dispose()
        {
            PollingTimer.Stop();
        }

        ~MouseModule()
        {
            Dispose();
        }

       
    }
}
