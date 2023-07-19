using EyeTribe.ClientSdk.Data;

namespace NITHdmis.Eyetracking.Eyetribe
{
    public interface IEyeTribeGazePointBehavior
    {
        void ReceiveGazePoint(GazeData e);
    }
}
