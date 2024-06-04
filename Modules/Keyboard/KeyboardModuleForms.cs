using RawInputProcessor;

namespace NITHdmis.Modules.Keyboard
{
    public class KeyboardModuleForms
    {
        private RawFormsInput _rawinput;

        public KeyboardModuleForms(IntPtr parentHandle)
        {
            _rawinput = new RawFormsInput(parentHandle, RawInputCaptureMode.ForegroundAndBackground);

            _rawinput.AddMessageFilter();

            _rawinput.KeyPressed += OnKeyPressed;
        }

        /// <summary>
        /// Contains all the behavior modules set.
        /// </summary>
        public List<IKeyboardBehavior> KeyboardBehaviors { get; set; } = new List<IKeyboardBehavior>();

        private void OnKeyPressed(object sender, RawInputEventArgs e)
        {
            foreach (IKeyboardBehavior behavior in KeyboardBehaviors)
            {
                behavior.ReceiveEvent(e);
            }
        }
    }
}