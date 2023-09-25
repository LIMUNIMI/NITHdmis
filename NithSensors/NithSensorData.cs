using System.Collections.Generic;

namespace NITHdmis.NithSensors
{
    public class NithSensorData
    {
        public string RawLine { get; set; } = string.Empty;
        public NithSensorNames SensorName { get; set; }
        public string Version { get; set; }
        public NithStatusCodes StatusCode { get; set; }
        public Dictionary<NithArguments, NithValue> Values { get; set; }
        public NithSensorData()
        {
            SensorName = NithSensorNames.NaS;
            Version = "";
            StatusCode = NithStatusCodes.NaC;
            Values = new Dictionary<NithArguments, NithValue>();
        }

        public void Reset()
        {
            SensorName = NithSensorNames.NaS;
            Version = "";
            StatusCode = NithStatusCodes.NaC;
            Values.Clear();
        }
        public NithValue? GetArgument(NithArguments argument)
        {
            NithValue ret;
            Values.TryGetValue(argument, out ret);
            return ret;
        }
        public bool ContainsArgument(NithArguments requiredArgument)
        {
            bool ret = true;

            if (!Values.ContainsKey(requiredArgument))
            {
                ret = false;
            }
            return ret;
        }
        public bool ContainsArguments(List<NithArguments> requiredArguments)
        {
            bool ret = true;
            foreach(NithArguments argument in requiredArguments)
            {
                if (!Values.ContainsKey(argument))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
    }
}
