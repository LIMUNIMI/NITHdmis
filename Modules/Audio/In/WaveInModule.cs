using NAudio.Wave;

namespace NITHdmis.Modules.Audio.In
{
    public class WaveInModule : IDisposable, IAudioInParamsListener
    {
        private AudioInParameters audioInParameters;

        private int deviceIndex = 0;

        private bool enabled = false;

        private short[] PcmData;

        private WaveInEvent waveInEvent;

        public WaveInModule(AudioInParameters audioInParameters)
        {
            this.audioInParameters = audioInParameters;
        }

        public AudioInParameters AudioInParameters { get => audioInParameters; set => audioInParameters = value; }

        public List<IPcmDataReceiver> PcmDataReceivers { get; set; } = new List<IPcmDataReceiver>();

        public int WaveInDeviceIndex
        {
            get { return deviceIndex; }
            set { deviceIndex = value; InitializeWaveIn(); }
        }

        public bool WaveInEnabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                switch (value)
                {
                    case true: StartWaveIn(); break;
                    case false: StopWaveIn(); break;
                    default: break;
                }
            }
        }

        public void Dispose()
        {
            StopWaveIn();
            waveInEvent?.Dispose();
        }

        public void ReceiveAudioInParams(AudioInParameters audioInParameters)
        {
            this.audioInParameters = audioInParameters;
            InitializeWaveIn();
        }

        private void InitializeWaveIn()
        {
            waveInEvent?.StopRecording();
            waveInEvent?.Dispose();

            waveInEvent = new WaveInEvent
            {
                DeviceNumber = deviceIndex,
                WaveFormat = new WaveFormat(audioInParameters.SampleRate, audioInParameters.BitRate, audioInParameters.Channels),
                BufferMilliseconds = audioInParameters.BufferMilliseconds
            };
            waveInEvent.DataAvailable += OnWaveInDataAvailable;
        }

        private void NotifyPcmListeners()
        {
            foreach (IPcmDataReceiver l in PcmDataReceivers)
            {
                l.ReceivePCMData(PcmData);
            }
        }

        private void OnWaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            int bytesPerSample = waveInEvent.WaveFormat.BitsPerSample / 8;
            int samplesRecorded = e.BytesRecorded / bytesPerSample;
            if (PcmData == null)
                PcmData = new short[samplesRecorded];
            //MessageBox.Show(samplesRecorded.ToString());
            for (int i = 0; i < samplesRecorded; i++)
                PcmData[i] = BitConverter.ToInt16(e.Buffer, i * bytesPerSample);

            NotifyPcmListeners();
        }

        private void StartWaveIn()
        {
            if (waveInEvent != null)
            {
                waveInEvent.StopRecording();
            }

            InitializeWaveIn();
            waveInEvent.StartRecording();
        }

        private void StopWaveIn()
        {
            if (waveInEvent != null)
                waveInEvent.StopRecording();
        }
    }
}