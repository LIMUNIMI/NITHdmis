namespace NITHdmis.Utils
{
    public class SegmentMapper
    {
        private double Offset;

        private double BaseSpan;

        private double TargetSpan;

        public SegmentMapper(double baseMin, double baseMax, double targetMin, double targetMax)
        {
            BaseMin = baseMin;
            BaseMax = baseMax;
            TargetMin = targetMin;
            TargetMax = targetMax;

            Offset = TargetMin - BaseMin;
            BaseSpan = BaseMax - BaseMin;
            TargetSpan = TargetMax - TargetMin;
        }

        public double BaseMin { get; set; }
        public double BaseMax { get; set; }
        public double TargetMin { get; set; }
        public double TargetMax { get; set; }

        public double Map(double value)
        {
            double valueScaled = (value - BaseMin) / BaseSpan;

            return TargetMin + (valueScaled * TargetSpan);
        }
    }
}