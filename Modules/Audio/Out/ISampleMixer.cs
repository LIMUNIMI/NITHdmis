using NAudio.Wave;

namespace NITHdmis.Modules.Audio.Out
{
    public interface ISampleMixer
    {
        WaveFormat GetWaveFormat();
        void ReceiveSampleUpdate(ISampleProvider oldSampleProvider, ISampleProvider newSampleProvider);
    }
}