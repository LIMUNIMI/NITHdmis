using NAudio.Wave;

namespace NITHdmis.Modules.Audio.Out
{
    public abstract class AWaveSampleMaker : IDeviceMixerListener
    {
        private ISampleProvider oldSampleProvider;
        private ISampleProvider newSampleProvider;
        protected ISampleProvider candidateSampleProvider;

        protected ISampleMixer sampleMixer;

        protected AWaveSampleMaker(ISampleMixer sampleMixer)
        {
            this.sampleMixer = sampleMixer;
        }

        protected void NotifySampleChanged()
        {
            oldSampleProvider = newSampleProvider;
            newSampleProvider = candidateSampleProvider;
            sampleMixer.ReceiveSampleUpdate(oldSampleProvider, newSampleProvider);
        }

        /// <summary>
        /// Describes what to do when the Mixer signals that the device has changed
        /// </summary>
        public abstract void ReceiveDeviceChanged();
    }
}
