using System.Globalization;

namespace NITHdmis.NithSensors
{
    public struct NithValue
    {
        public NithValue(string inputString)
        {
            string[] chunks = inputString.Split('/');
            if(chunks.Length == 1) // Only Value case
            {
                Type = NithDataTypes.OnlyValue;
                Base = chunks[0];
                Max = "";
                Proportional = double.NaN;
            }
            else // Value and Max case
            {
                Type = NithDataTypes.ValueAndMax;
                Base = chunks[0];
                Max = chunks[1];
                Proportional = ((double.Parse(Base, CultureInfo.InvariantCulture) * 100) / double.Parse(Max, CultureInfo.InvariantCulture));
            }
            
        }

        public string Base { get; set; }
        public string Max { get; set; }
        public double Proportional { get; set; }
        public NithDataTypes Type { get; set; }
    }

    public enum NithDataTypes
    {
        OnlyValue,
        ValueAndMax
    }
}
