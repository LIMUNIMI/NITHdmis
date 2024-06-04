namespace NITHdmis.Modules.Audio.Out
{
    /// <summary>
    /// Describes a wave out device's main characteristics. Designed to be used as a data type.
    /// </summary>
    public class WaveOutDeviceInfo
    {
        public WaveOutDeviceInfo(int waveOutDeviceIndex, int numberOfBuffers, int desiredLatency)
        {
            WaveOutDeviceIndex = waveOutDeviceIndex;
            NumberOfBuffers = numberOfBuffers;
            DesiredLatency = desiredLatency;
        }

        public WaveOutDeviceInfo() { }

        /// <summary>
        /// Index among system devices list
        /// </summary>
        public int WaveOutDeviceIndex { get; set; }
        /// <summary>
        /// Number of buffers, e.g. 1 for mono, 2 for stereo
        /// </summary>
        public int NumberOfBuffers { get; set; }
        /// <summary>
        /// Desired latency in ms, which will probably affect buffer length
        /// </summary>
        public int DesiredLatency { get; set; }
    }
}
