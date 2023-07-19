namespace NITHdmis.NithSensors
{
    public interface INithErrorBehavior
    {
        bool HandleError(NithErrors error);
    }
}
