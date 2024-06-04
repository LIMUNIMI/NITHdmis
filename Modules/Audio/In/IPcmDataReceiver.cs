namespace NITHdmis.Modules.Audio.In
{

    /// <summary>
    /// Represents a public interface for receiving PCM audio data.
    /// </summary>
    public interface IPcmDataReceiver
    {
        /// <summary>
        /// Receives the audio input parameters.
        /// </summary>
        /// <param name="audioInParameters">The audio input parameters to receive.</param>
        void ReceiveAudioInParams(AudioInParameters audioInParameters);

        /// <summary>
        /// Receives the PCM audio data.
        /// </summary>
        /// <param name="pcmData">The PCM audio data to receive as an array of short.</param>
        void ReceivePCMData(short[] pcmData);
    }
}
