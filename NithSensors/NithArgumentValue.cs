using System.Globalization;

namespace NITHdmis.NithSensors
{
    public enum NithDataTypes
    {
        OnlyValue,
        ValueAndMax
    }

    public struct NithArgumentValue
    {
        public NithArgumentValue(NithArguments argument, string valueString)
        {
            string[] chunks = valueString.Split('/');
            if (chunks.Length == 1) // Only Value case
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
                Max = chunks[1].Replace("\n", "");
                Proportional = (double.Parse(Base, CultureInfo.InvariantCulture) * 100) / double.Parse(Max, CultureInfo.InvariantCulture);
            }

            Argument = argument;
        }

        public NithArgumentValue(NithArguments argument, string Base, string Max)
        {
            Type = NithDataTypes.ValueAndMax;
            this.Base = Base;
            this.Max = Max.Replace("\n", "");
            Proportional = (double.Parse(Base, CultureInfo.InvariantCulture) * 100) / double.Parse(Max, CultureInfo.InvariantCulture);

            Argument = argument;
        }

        public NithArguments Argument { get; set; }
        public string Base { get; set; }
        public string Max { get; set; }
        public double Proportional { get; set; }
        public NithDataTypes Type { get; set; }
    }
}