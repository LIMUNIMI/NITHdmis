using NITHlibrary.Nith.Internals;
using System.Drawing;

namespace NITHdmis.Modules.Mouse
{
    public class NithSensorBehavior_GazeToMouse : INithSensorBehavior
    {
        private MouseModule mouseModule;
        public bool Enabled { get; set; } = false;

        private List<NithParameters> requiredParams = new List<NithParameters>
        {
            NithParameters.gaze_x, NithParameters.gaze_y
        };

        public NithSensorBehavior_GazeToMouse(MouseModule mouseModule, bool enabled = false)
        {
            this.mouseModule = mouseModule;
            Enabled = enabled;
        }

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(requiredParams) && Enabled)
            {
                int gaze_x = (int)nithData.GetParameter(NithParameters.gaze_x).Value.Base_AsDouble;
                int gaze_y = (int)nithData.GetParameter(NithParameters.gaze_y).Value.Base_AsDouble;
                mouseModule.SetCursorPosition(new Point(gaze_x, gaze_y));
            }
        }
    }
}
