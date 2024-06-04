namespace NITHdmis.Modules.MIDI
{
    /// <summary>
    /// Basic interfaces for MIDI controller modules.
    /// </summary>
    public interface IMidiModule
    {
        void PlayNote(int note, int velocity);
        void StopNote(int note);
        int OutDevice { get; set; }
        int MidiChannel { get; set; }
        bool IsMidiOk();
        void SetPressure(int pressure);
        void SetModulation(int modulation);
        void SetPitchBend(int pitchBendValue);
        void SetPitchNoBend();
        void SendMessage(int byte1, int byte2, int byte3);
        void SetExpression(int expression);
    }
}
