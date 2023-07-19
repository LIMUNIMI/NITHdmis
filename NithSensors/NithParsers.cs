using System;

namespace NITHdmis.NithSensors
{
    internal static class NithParsers
    {
        public static NithStatusCodes ParseStatusCode(string code)
        {
            NithStatusCodes ret = NithStatusCodes.NaC;
            try
            {
                ret = (NithStatusCodes)Enum.Parse(typeof(NithStatusCodes), code);
            }
            catch
            {

            }
            return ret;
        }

        public static NithArguments ParseField(string field)
        {
            NithArguments ret = NithArguments.NaF;
            try
            {
                ret = (NithArguments)Enum.Parse(typeof(NithArguments), field);
            }
            catch
            {

            }
            return ret;
        }

        public static NithSensorNames ParseSensorName(string name)
        {
            NithSensorNames ret = NithSensorNames.NaS;
            try
            {
                ret = (NithSensorNames)Enum.Parse(typeof(NithSensorNames), name);
            }
            catch
            {

            }
            return ret;
        }
    }
}
