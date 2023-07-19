using Tobii.Interaction;

namespace NITHdmis.Eyetracking.Tobii
{
    public interface ITobiiGazePointBehavior
    {
        void ReceiveGazePoint(GazePointData e);
    }
}