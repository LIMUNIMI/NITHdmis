using System.Windows.Interop;
using RawInputProcessor;

namespace NITHdmis.Modules.Keyboard
{
    /// <summary>
    /// Represents a keyboard module for WPF applications.
    /// </summary>
    public sealed class KeyboardModuleWPF
    {
        private RawPresentationInput _rawinput;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardModuleWPF"/> class.
        /// </summary>
        /// <param name="parentHandle">The handle of the parent window.</param>
        /// <param name="captureMode">The capture mode for raw input.</param>
        public KeyboardModuleWPF(IntPtr parentHandle, RawInputCaptureMode captureMode)
        {
            _rawinput = new RawPresentationInput(HwndSource.FromHwnd(parentHandle), captureMode);

            _rawinput.AddMessageFilter();

            _rawinput.KeyPressed += OnKeyPressed;
        }

        /// <summary>
        /// Gets or sets the list of keyboard behaviors.
        /// </summary>
        public List<IKeyboardBehavior> KeyboardBehaviors { get; set; } = new List<IKeyboardBehavior>();

        /// <summary>
        /// Handles the KeyPressed event of the raw input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RawInputEventArgs"/> instance containing the event data.</param>
        private void OnKeyPressed(object sender, RawInputEventArgs e)
        {
            foreach (IKeyboardBehavior behavior in KeyboardBehaviors)
            {
                behavior.ReceiveEvent(e);
            }
        }
    }
}
