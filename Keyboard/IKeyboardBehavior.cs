using RawInputProcessor;

namespace NITHdmis.Keyboard
{
    public interface IKeyboardBehavior
    {
        int ReceiveEvent(RawInputEventArgs e);
    }
}