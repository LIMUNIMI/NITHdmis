using RawInputProcessor;

namespace NITHdmis.Modules.Keyboard
{
    public interface IKeyboardBehavior
    {
        int ReceiveEvent(RawInputEventArgs e);
    }
}