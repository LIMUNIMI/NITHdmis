using Tobii.Interaction;

namespace NITHdmis.Eyetracking.Tobii
{
    public interface ITobiiEyePositionBehavior
    {
        void ReceiveEyePositionData(EyePositionData e);
    }
}