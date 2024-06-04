using NAudio.Wave.SampleProviders;

namespace NITHdmis.Modules.Audio.Out
{
    public class MuteSignalGenerator : SignalGenerator
    {
        public MuteSignalGenerator(int sampleRate, int channels) : base(sampleRate, channels)
        {
            Gain = 0;
        }
    }
}
