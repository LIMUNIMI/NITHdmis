using Tobii.Interaction;
using Tobii;

namespace NITHdmis.Eyetracking.Tobii
{
    public interface ITobiiHeadPoseBehavior
    {
        void ReceiveHeadPoseData(HeadPoseData data);
    }
}