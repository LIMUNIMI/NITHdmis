namespace NITHdmis.Modules.MIDI
{
    /// <summary>
    /// Some utilities to set the MIDI module.
    /// </summary>
    public class MidiDeviceFinder
    {
        private IMidiModule midiModule;

        public MidiDeviceFinder(IMidiModule midiModule)
        {
            this.midiModule = midiModule;
        }

        public void SetToLastDevice()
        {
            midiModule.OutDevice = 50;
            while (!midiModule.IsMidiOk())
            {
                midiModule.OutDevice--;
            }
        }

    }
}
