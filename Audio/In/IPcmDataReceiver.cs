using System;

namespace NITHdmis.Audio.In
{
    public interface IPcmDataReceiver
    {
        void ReceiveAudioInParams(AudioInParameters audioInParameters);
        void ReceivePCMData(short[] pcmData);
    }
}
