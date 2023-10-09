using NAudio.Midi;

namespace NITHdmis.MIDI
{
    /// <summary>
    /// A MIDI controller module based on the library NAudio.
    /// </summary>
    public class MidiModuleNAudio : IMidiModule
    {
        private int midiChannel = 0;
        private bool midiOk = false;
        private MidiOut midiOut;
        private int outDevice = 0;
        public int MidiChannel { get => midiChannel; set { midiOk = false;  midiChannel = value; ResetMidiOut(); } }
        public int OutDevice { get => outDevice; set { midiOk = false; outDevice = value; ResetMidiOut(); } }

        public MidiModuleNAudio(int outDevice, int midiChannel)
        {
            this.outDevice = outDevice;
            this.midiChannel = midiChannel;


            ResetMidiOut();
        }

        public bool IsMidiOk()
        {
            return (midiOk);
        }

        public void PlayNote(int pitch, int velocity)
        {
            if(midiOut!=null && midiOk)
            midiOut.Send(MidiMessage.StartNote(pitch, velocity, MidiChannel).RawData);
        }

        public void SendMessage(int byte1, int byte2, int byte3)
        {
            if(midiOut!=null && midiOk)
            midiOut.Send(MidiMessage.ChangeControl(byte1, byte2, midiChannel).RawData);
        }

        public void SetExpression(int expression)
        {
            if(midiOut!=null && midiOk)
            midiOut.Send(MidiMessage.ChangeControl(11, expression, midiChannel).RawData);
        }

        public void SetModulation(int modulation)
        {
            if(midiOut!=null && midiOk)
                midiOut.Send(MidiMessage.ChangeControl(1, modulation, midiChannel).RawData);
        }

        public void SetPitchBend(int pitchBendValue)
        {
            // Set limits
            if (pitchBendValue > 16383)
                pitchBendValue = 16383;
            if (pitchBendValue < 0)
                pitchBendValue = 0;

            int lsb = pitchBendValue & 0b1111111;
            int msb = (pitchBendValue & 0b1111111_0000000) >> 7;
            int status = 0b1110 << 4;

            MidiMessage message = new MidiMessage(status, lsb, msb);

            if(midiOut!=null && midiOk)
                midiOut.Send(message.RawData);
        }

        public void SetPitchNoBend()
        {
            if(midiOut!=null && midiOk)
                SetPitchBend(8192);
        }

        public void SetPressure(int pressure)
        {
            if(midiOut!=null && midiOk)
            midiOut.Send(MidiMessage.ChangeControl(7, pressure, midiChannel).RawData);
        }

        public void StopNote(int pitch)
        {
            if(midiOut!=null && midiOk)
            midiOut.Send(MidiMessage.StopNote(pitch, 0, MidiChannel).RawData);
        }

        private void ResetMidiOut()
        {
            if(midiOut != null)
            {
                midiOut.Close();
                midiOut.Dispose();
            }
            try
            {
                midiOut = new MidiOut(this.OutDevice);
                midiOk = true;
            }
            catch
            {
                midiOk = false;
            }
        }
    }
}