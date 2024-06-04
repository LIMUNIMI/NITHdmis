namespace NITHdmis.Modules.Mouse
{
    public class MouseModuleSample
    {
        public MouseModuleSample()
        {
        }

        public MouseModuleSample(double velocityX, double velocityY, double positionX, double positionY, int directionX, int directionY)
        {
            VelocityX = velocityX;
            VelocityY = velocityY;
            PositionX = positionX;
            PositionY = positionY;
            DirectionX = directionX;
            DirectionY = directionY;
        }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }
    }
}