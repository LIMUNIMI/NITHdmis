using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace NITHdmis.Modules.Audio.In
{
    // TODO WIP
    /// <summary>
    /// WORK IN PROGRESS. A class to help extract device characteristics and define AudioInParameters given that.
    /// This will hopefully replace the AudioInParamsUnifier.
    /// </summary>
    public class AudioInHelper
    {
        public void SendWaveInChange()
        {
            WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(DeviceIndex);
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            MMDevice defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            int sampleRate = defaultDevice.AudioClient.MixFormat.SampleRate;

        }

        public int DeviceIndex { get; set; } = 0;
        public int SampleRate { get; set; } = 48_000;
        public int BufferMilliseconds { get; set; } = 43;
        public ZeroPaddingModes ZeroPaddingMode { get; set; }
    }
}
