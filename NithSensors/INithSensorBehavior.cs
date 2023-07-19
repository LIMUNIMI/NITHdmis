namespace NITHdmis.NithSensors
{
    public interface INithSensorBehavior
    {
        void HandleData(NithSensorData nithData);
    }
}