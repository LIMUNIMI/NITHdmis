using NITHdmis.ATmega;
using System.Globalization;

namespace NITHdmis.Headtracking.NeeqHT
{
    public class SBneeqHTBehavior : ISensorBehavior
    {
        private NeeqHTModule module;
        private string[] split;

        public SBneeqHTBehavior(NeeqHTModule module)
        {
            this.module = module;
        }

        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%"))
            {
                module.Write("A"); // Wakes up the headtracker
            }
            else if (val.StartsWith("$"))
            {
                module.HeadTrackerMode = HeadTrackerModes.Absolute;

                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                module.Data.Position = new Polar3DData 
                {
                    Yaw = double.Parse(split[0], CultureInfo.InvariantCulture), 
                    Roll = double.Parse(split[1], CultureInfo.InvariantCulture), 
                    Pitch = double.Parse(split[2], CultureInfo.InvariantCulture) 
                };
            }
            else if (val.StartsWith("R"))
            {
                module.HeadTrackerMode = HeadTrackerModes.Acceleration;

                val = val.Replace("R", string.Empty);
                split = val.Split('!');

                module.Data.Acceleration = new Polar3DData
                {
                    Pitch = double.Parse(split[0], CultureInfo.InvariantCulture),
                    Roll = double.Parse(split[1], CultureInfo.InvariantCulture),
                    Yaw = double.Parse(split[2], CultureInfo.InvariantCulture)
                };
                
            }

            foreach (INeeqHTbehavior b in module.Behaviors)
            {
                b.ReceiveHeadTrackerData(module.Data);
            }
        }
    }
}