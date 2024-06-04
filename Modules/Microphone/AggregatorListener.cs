namespace NITHdmis.Modules.Microphone
{
    internal interface AggregatorListener
    {
        void aggregatorReached(float min, float max, float avg);
    }
}
