using NITHdmis.Music;

namespace NITHdmis.Modules.Audio.In
{
    /// <summary>
    /// A powerful unified class to manage all the AudioIn parameters and calculations.
    /// Given sampleRate, bitRate, channels, bufferMilliseconds (and eventually zeroPaddingMode), it will calculate all the parameters necessary to perform FFT (Fast Fourier Transform), as well as exposing methods to perform FFT calculations.
    /// </summary>
    public partial class AudioInParameters
    {
        private short[] ZeroPaddedArray;

        // TODO I don't like the fact a new instance is needed... This could be fixed through encapsulation and getters/setters
        /// <summary>
        /// Build an AudioInParameters instance. Take in consideration you'll need to make a new instance to modify each parameter...
        /// </summary>
        /// <param name="sampleRate">Sample rate of the audio in device</param>
        /// <param name="bitRate">Bit rate of the audio in device</param>
        /// <param name="channels">Number of channels of the audio in device</param>
        /// <param name="bufferMilliseconds">Desired buffer length in milliseconds. Higher values mean more lag, but higher FFT resolution (more bins),since the FFT input array will be longer.</param>
        /// <param name="zeroPaddingMode">An FFT technique which consists in adding zeroes to the FFT input array. It is used to force a higher FFT resolution (to obtain more bins), but it doesn't add any real information to the in spectrum. Mostly, it's used to reach power-of-two array lengths, which are needed for FFT.</param>
        public AudioInParameters(int sampleRate, int bitRate, int channels, int bufferMilliseconds, ZeroPaddingModes zeroPaddingMode = ZeroPaddingModes.Absent)
        {
            SampleRate = sampleRate;
            BitRate = bitRate;
            Channels = channels;
            BufferMilliseconds = bufferMilliseconds;
            ZeroPaddingMode = zeroPaddingMode;

            // Calculations
            PcmDataLength = (int)(SampleRate * ((double)BufferMilliseconds / 1000f));

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

        public int BitRate { get; private set; }
        public int BufferMilliseconds { get; private set; }

        public int Channels { get; private set; }

        public int FftPoints { get; private set; }

        public int PcmDataLength { get; }

        public int SampleRate { get; private set; }
        public short[] ZeroesArray { get; private set; }

        public int ZeroPaddedArrayLength { get; private set; }

        public ZeroPaddingModes ZeroPaddingMode { get; private set; }

        /// <summary>
        /// Given an FFT bin, returns the central frequency (in Hz) it is mapped to.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public double BinToFrequency(int bin)
        {
            double maxFreq = SampleRate;
            return bin * (maxFreq / ZeroPaddedArrayLength);
        }

        /// <summary>
        /// Given a midiNote, returns the FFT bin which better corresponds to that note.
        /// </summary>
        /// <param name="midiNote"></param>
        /// <returns></returns>
        public int MidiNoteToBin(MidiNotes midiNote)
        {
            int maxFreq = SampleRate;
            return (int)(ZeroPaddedArrayLength * midiNote.GetFrequency() / maxFreq);
        }

        /// <summary>
        /// Applies zeropadding to a given pcmData array, using the zeropadding technique specified before.
        /// </summary>
        /// <param name="pcmData"></param>
        /// <returns></returns>
        public short[] ZeroPad(short[] pcmData)
        {
            pcmData.CopyTo(ZeroPaddedArray, 0);
            ZeroesArray.CopyTo(ZeroPaddedArray, pcmData.Length);
            return ZeroPaddedArray;
        }
    }
}