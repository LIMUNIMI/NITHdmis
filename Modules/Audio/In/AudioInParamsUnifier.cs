namespace NITHdmis.Modules.Audio.In
{
    /// <summary>
    /// A tool to make sure every AudioInParameters listener is working on the same AudioInParameters.
    /// Add Listeners, then call NotifyParamsToReceivers to update them.
    /// </summary>
    public class AudioInParamsUnifier
    {
        public AudioInParamsUnifier(List<IAudioInParamsListener> listeners)
        {
            Listeners = listeners;
        }

        public AudioInParamsUnifier()
        {
        }

        public AudioInParamsUnifier(AudioInParameters audioInParams)
        {
            AudioInParams = audioInParams;
        }

        public AudioInParameters AudioInParams { get; set; }

        public List<IAudioInParamsListener> Listeners { get; set; } = new List<IAudioInParamsListener>();

        public void NotifyParamsToReceivers()
        {
            foreach (IAudioInParamsListener listener in Listeners)
            {
                listener.ReceiveAudioInParams(AudioInParams);
            }
        }
    }
}