using NAudio.Midi;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
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

            public int MidiChannel { get => midiChannel; set { midiOk = false; midiChannel = value; ResetMidiOut(); } }
            public int OutDevice { get => outDevice; set { midiOk = false; outDevice = value; ResetMidiOut(); } }

            public MidiModuleNAudio(int outDevice, int midiChannel)
            {
                this.outDevice = outDevice;
                this.midiChannel = midiChannel;
                ResetMidiOut();
            }

            public bool IsMidiOk()
            {
                return midiOk;
            }

            public void PlayNote(int pitch, int velocity)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.StartNote(pitch, velocity, midiChannel).RawData);
            }

            public void SendMessage(int byte1, int byte2, int byte3)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(byte1, byte2, midiChannel).RawData);
            }

            public void SetExpression(int expression)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(11, expression, midiChannel).RawData);
            }

            public void SetModulation(int modulation)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(1, modulation, midiChannel).RawData);
            }

            public void SetModulationRate(int modulationRate)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(19, modulationRate, midiChannel).RawData);
            }

            public void ToggleTremolo(bool activate, int baseNote, int range)
            {
                int tremoloValue = activate ? 127 : 0; // 127 represents full activation, 0 full deactivation
                if (activate)
                {
                    int trillNote = baseNote + range;
                    // Send both notes to the MIDI module to handle the trill effect
                    SetTrillNotes(baseNote, trillNote);
                }
                SetTremolo(tremoloValue);
            }

            public void SetTremolo(int tremolo)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(13, tremolo, midiChannel).RawData);
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

                if (midiOut != null && midiOk)
                    midiOut.Send(message.RawData);
            }

            public void SetPitchNoBend()
            {
                if (midiOut != null && midiOk)
                    SetPitchBend(8192);
            }

        public void SetAttackRampSpeed(int attack)
        {
            if (midiOut != null && midiOk)
                midiOut.Send(MidiMessage.ChangeControl(9, attack, midiChannel).RawData);
        }

        public void SetPressure(int pressure)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.ChangeControl(8, pressure, midiChannel).RawData);
            }

            public void SetPanic()
            {
                if (midiOut != null && midiOk)
                {
                    midiOut.Send(MidiMessage.ChangeControl(127, 127, 15).RawData);
                }
            }

            public void StopNote(int pitch)
            {
                if (midiOut != null && midiOk)
                    midiOut.Send(MidiMessage.StopNote(pitch, 0, midiChannel).RawData);
            }

            private bool switchTrill = false;

            private void SetTrillNotes(int baseNote, int trillNote)
            {
                if (midiOut != null && midiOk)
                {
                    if (switchTrill)
                    {
                        PlayNote(baseNote, 100); // Adjust velocity as needed
                    }
                    else
                    {
                        PlayNote(trillNote, 100); // Adjust velocity as needed
                    }
                    switchTrill = !switchTrill;
                }
            }

            private void ResetMidiOut()
            {
                if (midiOut != null)
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

            public void PlayNote(object tag, int v)
            {
                throw new NotImplementedException();
            }
        }
    }
