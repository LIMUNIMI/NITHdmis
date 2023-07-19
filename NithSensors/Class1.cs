using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.NithSensors
{
    internal class TBPressureToVelocity : INithSensorBehavior
    {
        public void HandleData(NithSensorData nithData)
        {
            double velocity = double.Parse(nithData.GetValue(NithArguments.press), CultureInfo.InvariantCulture);
        }
    }
}
