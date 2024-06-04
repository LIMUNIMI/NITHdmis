namespace NITHdmis.Modules.Mouse
{
    public interface IMouseBehavior
    {
        int ReceiveSample(MouseModuleSample sample);
    }
}