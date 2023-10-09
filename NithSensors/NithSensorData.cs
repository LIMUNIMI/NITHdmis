using System.Collections.Generic;

namespace NITHdmis.NithSensors
{
    public class NithSensorData
    {
        public string RawLine { get; set; } = string.Empty;
        public NithSensorNames SensorName { get; set; }
        public string Version { get; set; }
        public NithStatusCodes StatusCode { get; set; }
        public List<NithArgumentValue> Values { get; set; }
        public NithSensorData()
        {
            SensorName = NithSensorNames.NaS;
            Version = "";
            StatusCode = NithStatusCodes.NaC;
            Values = new List<NithArgumentValue>();
        }

        public void Reset()
        {
            SensorName = NithSensorNames.NaS;
            Version = "";
            StatusCode = NithStatusCodes.NaC;
            Values.Clear();
        }
        public NithArgumentValue? GetArgument(NithArguments argument)
        {
            foreach(NithArgumentValue value in Values)
            {
                if(value.Argument == argument)
                {
                    return value;
                }
            }
            return null;
        }

        public bool ContainsArgument(NithArguments argument)
        {
            foreach(NithArgumentValue value in Values)
            {
                if(value.Argument == argument)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsArguments(List<NithArguments> arguments)
        {
            foreach(NithArguments argument in arguments)
            {
                if (!ContainsArgument(argument))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
