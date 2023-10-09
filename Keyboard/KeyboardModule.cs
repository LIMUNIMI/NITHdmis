using RawInputProcessor;
using System;
using System.Collections.Generic;
using System.Windows.Interop;

namespace NITHdmis.Keyboard
{
    public sealed class KeyboardModule
    {
        private RawPresentationInput _rawinput;

        ///<summary>
        /// Initializes a new instance of the KeyboardModule class.
        ///</summary>
        ///<param name="parentHandle">The handle of the parent window.</param>
        ///<param name="captureMode">The mode to capture raw input.</param>
        public KeyboardModule(IntPtr parentHandle, RawInputCaptureMode captureMode)
        {
            _rawinput = new RawPresentationInput(HwndSource.FromHwnd(parentHandle), captureMode);

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