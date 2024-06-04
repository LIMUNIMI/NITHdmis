using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace NITHdmis.Modules.Audio.Out
{
    /// <summary>
    /// Defines a WaveOut device, and controls the sample it's reproducing.
    /// It accepts a list of various samples (ISampleProvider), and automatically mixes them.
    /// </summary>
    public class WaveOutDeviceMixerModule : IDisposable, ISampleMixer
    {
        private const float DEFAULT_MASTER_PANNING = 0f;
        private const float DEFAULT_MASTER_VOLUME = 0.8f;
        private int desiredLatency;
        private bool enabled = false;
        private MixingSampleProvider masterMixProvider;
        private float masterVolume = DEFAULT_MASTER_VOLUME;
        private int waveOutDeviceIndex;
        private WaveOutEvent waveOutEvent;
        private SampleToWaveProvider sampleToWaveProvider;
        private WaveFormat waveFormat;
        private List<ISampleProvider> muteGeneratorsList;

        
        public WaveOutDeviceMixerModule(WaveFormat waveFormat, int waveOutDeviceIndex = 0, int desiredLatency = 100)
        {
            this.waveOutDeviceIndex = waveOutDeviceIndex;
            this.desiredLatency = desiredLatency;

            this.waveFormat = waveFormat;
            muteGeneratorsList = new List<ISampleProvider>()
            {
                new MuteSignalGenerator(waveFormat.SampleRate, waveFormat.Channels)
            };

            masterMixProvider = new MixingSampleProvider(muteGeneratorsList);
            masterMixProvider.RemoveAllMixerInputs();

            BuildWaveOutEvent();

            InitWaveOutChain();
        }

        ///<summary>
        /// Initializes the WaveOut chain by creating a SampleToWaveProvider and initializing the WaveOutEvent with it.
        ///</summary>
        public void InitWaveOutChain()
        {
            sampleToWaveProvider = new SampleToWaveProvider(masterMixProvider);
            waveOutEvent.Volume = masterVolume;
            waveOutEvent.Init(sampleToWaveProvider);
        }


        public int DesiredLatency { get => desiredLatency; set => desiredLatency = value; }

        /// <summary>
        /// General killswitch for this entire module. Inits the WaveOutEvent if switched to enable, or stops it conversely. Effect: turn on/off the sound.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value != enabled)
                {
                    enabled = value;
                    switch (enabled)
                    {
                        case true:
                            waveOutEvent.Play();
                            break;

                        case false:
                            waveOutEvent.Stop();
                            break;
                    }
                }
            }
        }

        //public float MasterPanning
        //{
        //    get { return masterPanning; }
        //    set
        //    {
        //        masterPanning = value;
        //        masterPanProvider.Pan = value;
        //        ReInitWaveOutEvent();
        //    }
        //}
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                waveOutEvent.Volume = masterVolume;
                InitWaveOutChain();
            }
        }

        public int WaveOutDeviceIndex
        {
            get { return waveOutDeviceIndex; }
            set
            {
                waveOutDeviceIndex = value;
                InitWaveOutChain();
                NotifyDeviceChanged();
            }
        }

        public WaveOutEvent WaveOutEvent { get; set; }

        public List<IDeviceMixerListener> DeviceMixerListeners { get; set; } = new List<IDeviceMixerListener>();

        /// <summary>
        /// Change the WaveFormat. On change, reinitialize and notify listeners.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get => waveFormat;
            set
            {
                waveFormat = value;
                InitWaveOutChain();
                NotifyDeviceChanged();
            }
        }

        
        public void Dispose()
        {
            if (waveOutEvent != null)
            {
                waveOutEvent.Stop();
                waveOutEvent.Dispose();
            }
        }


        private void NotifyDeviceChanged()
        {
            foreach (IDeviceMixerListener listener in DeviceMixerListeners)
            {
                listener.ReceiveDeviceChanged();
            }
        }

        /// <summary>
        /// Receives a sample from an ASampleMaker.
        /// </summary>
        /// <param name="oldSampleProvider">Old sample to be removed from the mix</param>
        /// <param name="newSampleProvider">New sample to be added to the mix</param>
        public void ReceiveSampleUpdate(ISampleProvider oldSampleProvider, ISampleProvider newSampleProvider)
        {
            if (masterMixProvider.MixerInputs.Contains(oldSampleProvider) && oldSampleProvider != null)
            {
                masterMixProvider.RemoveMixerInput(oldSampleProvider);
            }

            masterMixProvider.AddMixerInput(newSampleProvider);
            //waveOutEvent.Init(sampleToWaveProvider);
            //InitWaveOutChain();
            // TODO forse è necessario reinizializzare
        }

        ///<summary>
        ///Builds a WaveOutEvent object for audio playback.
        ///</summary>
        ///<remarks>
        ///This method disposes the existing waveOutEvent object if it is not null, and creates a new WaveOutEvent object for audio playback.
        ///</remarks>
        private void BuildWaveOutEvent()
        {
            if (waveOutEvent != null)
                waveOutEvent.Dispose();
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.NumberOfBuffers = waveFormat.Channels;
            waveOutEvent.DeviceNumber = waveOutDeviceIndex;
            waveOutEvent.DesiredLatency = desiredLatency;
        }

        public WaveFormat GetWaveFormat()
        {
            return waveFormat;
        }
    }
}