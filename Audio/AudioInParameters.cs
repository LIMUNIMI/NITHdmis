using NITHdmis.Music;

namespace NITHdmis.Utils
{
    public class AudioInParameters
    {
        private short[] ZeroPaddedArray;
        public int BitRate { get; private set; }
        public int BufferMilliseconds { get; private set; }

        public int Channels { get; private set; }

        public int FftPoints { get; private set; }

        public int PcmDataLength { get; }

        public int SampleRate { get; private set; } //
        public short[] ZeroesArray { get; private set; }

        public int ZeroPaddedArrayLength { get; private set; } //

        public ZeroPaddingModes ZeroPaddingMode { get; private set; } //

        public AudioInParameters(int sampleRate, int bitRate, int channels, int bufferMilliseconds, ZeroPaddingModes zeroPaddingMode)
        {
            SampleRate = sampleRate;
            BitRate = bitRate;
            Channels = channels;
            BufferMilliseconds = bufferMilliseconds;
            ZeroPaddingMode = zeroPaddingMode;

            // Calculations
            PcmDataLength = (int)((double)SampleRate * ((double)BufferMilliseconds / 1000f));

            // FftPoints
            FftPoints = 2;
            while (FftPoints <= PcmDataLength) // while (FftPoints * 2 <= PcmDataLength)
                FftPoints *= 2;

            // Needed ZeroPadding
            switch (ZeroPaddingMode)
            {
                case ZeroPaddingModes.FillToPowerOfTwo:
                    ZeroesArray = new short[FftPoints - PcmDataLength];
                    for (int i = 0; i < ZeroesArray.Length; i++)
                    {
                        ZeroesArray[i] = 0;
                    }
                    ZeroPaddedArrayLength = FftPoints;
                    break;

                case ZeroPaddingModes.FillAndDouble:
                    ZeroesArray = new short[FftPoints * 2 - PcmDataLength];
                    for (int i = 0; i < ZeroesArray.Length; i++)
                    {
                        ZeroesArray[i] = 0;
                    }
                    ZeroPaddedArrayLength = FftPoints * 2;
                    break;

                case ZeroPaddingModes.Absent:
                default:
                    ZeroesArray = new short[0];
                    ZeroPaddedArrayLength = FftPoints;
                    break;
            }

            ZeroPaddedArray = new short[ZeroPaddedArrayLength];
        }

        public double BinToFrequency(int bin)
        {
            double maxFreq = SampleRate;
            return bin * (maxFreq / ZeroPaddedArrayLength);
        }

        public int MidiNoteToBin(MidiNotes midiNote)
        {
            int maxFreq = SampleRate;
            return (int)(ZeroPaddedArrayLength * midiNote.GetFrequency() / maxFreq);
        }

        public short[] ZeroPad(short[] pcmData)
        {
            pcmData.CopyTo(ZeroPaddedArray, 0);
            ZeroesArray.CopyTo(ZeroPaddedArray, pcmData.Length);
            return ZeroPaddedArray;
        }

        public enum ZeroPaddingModes
        {
            Absent,
            FillToPowerOfTwo,
            FillAndDouble
        }
    }
}