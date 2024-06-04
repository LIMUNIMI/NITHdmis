using NAudio.Wave;

namespace NITHdmis.Modules.Audio.In
{
    public class WaveDevicesScanner
    {
        public List<WaveInCapabilities> WaveInDevices { get; private set; }
        public List<WaveOutCapabilities> WaveOutDevices { get; private set; }

        public WaveDevicesScanner()
        {
            WaveInDevices = new List<WaveInCapabilities>();
            WaveOutDevices = new List<WaveOutCapabilities>();
            Scan();
        }

        public void Scan()
        {
            WaveInDevices.Clear();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInDevices.Add(WaveIn.GetCapabilities(i));
            }

            WaveOutDevices.Clear();
            for(int i = 0;i < WaveOut.DeviceCount; i++)
            {
                WaveOutDevices.Add(WaveOut.GetCapabilities(i));
            }
        }

    }
}
