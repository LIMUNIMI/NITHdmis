using NITHdmis.ATmega;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;

namespace NITHdmis.NithSensors
{
    /// <summary>
    /// A module capable to handle any NITH sensor
    /// </summary>
    public class NithModule : SensorBase
    {
        private readonly char[] STARTSYMBOL = new char[] { '$' };
        /// <summary>
        /// Initializes a Nith sensor module.
        /// </summary>
        public NithModule() : base(115200, "COM")
        {
            SensorBehaviors = new List<INithSensorBehavior>();
            ErrorBehaviors = new List<INithErrorBehavior>();
            LastSensorData = new NithSensorData();
            LastError = NithErrors.NaE;
        }

        public NithErrors LastError { get; protected set; }
        public NithSensorData LastSensorData { get; protected set; }
        public List<INithSensorBehavior> SensorBehaviors { get; protected set; }
        public List<INithErrorBehavior> ErrorBehaviors { get; protected set; }
        public List<NithSensorNames> ExpectedSensorNames { get; set; } = new List<NithSensorNames>();
        public List<string> ExpectedVersions { get; set; } = new List<string>();
        public List<NithArguments> ExpectedArguments { get; set; } = new List<NithArguments>();

        protected override void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            NithSensorData data = new NithSensorData();
            NithErrors error = NithErrors.NaE;

            if (IsConnectionOk)
            {
                try
                {
                    string line = serialPort.ReadLine();

                    if (line.StartsWith("$"))
                    {
                        //string TEST = "";
                        error = NithErrors.OK; // Set to ok, then check if wrong
                        try
                        {
                            // Output splitting
                            string[] fields = line.Split('|');
                            string[] firstField = fields[0].Split('-');
                            string[] arguments = fields[2].Split('&');
                            //TEST = firstField[0];

                            // Parsings
                            data.RawLine = line;
                            data.SensorName = NithParsers.ParseSensorName(firstField[0].Trim(STARTSYMBOL));
                            data.Version = firstField[1];
                            data.StatusCode = NithParsers.ParseStatusCode(fields[1]);
                            foreach (string v in arguments)
                            {
                                string[] s = v.Split('=');
                                string argumentName = s[0];
                                string value = "";

                                // Check if there is a "/" for value proportion
                                if (s[1].Contains("/"))
                                {
                                    string[] propString = s[1].Split('/');
                                    double propVal = (double.Parse(propString[0], CultureInfo.InvariantCulture) * 100) / double.Parse(propString[1], CultureInfo.InvariantCulture);
                                    value = propVal.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    value = s[1];
                                }
                                
                                data.Values.Add(NithParsers.ParseField(argumentName), value);
                            }
                        }
                        catch
                        {
                            error = NithErrors.OutputCompliance;
                        }

                        // Further error checking

                        // Check name
                        if (ExpectedSensorNames.Contains(data.SensorName) || ExpectedSensorNames.Count == 0)
                        {
                            // Check version
                            if (ExpectedVersions.Contains(data.Version) || ExpectedVersions.Count == 0)
                            {
                                // Check status code
                                if (data.StatusCode != NithStatusCodes.ERR)
                                {
                                    // Check arguments
                                    if (ExpectedArguments.Count != 0)
                                    {
                                        foreach (NithArguments arg in ExpectedArguments)
                                        {
                                            if (!data.Values.ContainsKey(arg))
                                            {
                                                error = NithErrors.Values;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    error = NithErrors.StatusCode;
                                }
                            }
                            else
                            {
                                error = NithErrors.Version;
                            }
                        }
                        else
                        {
                            error = NithErrors.Name;
                        }
                    }
                    else
                    {
                        error = NithErrors.OutputCompliance;
                    }
                }
                catch
                {
                    error = NithErrors.Connection;
                }
            }
            else
            {
                error = NithErrors.Connection;
            }

            // Send to errorbehaviors
            foreach (INithErrorBehavior ebeh in ErrorBehaviors)
            {
                ebeh.HandleError(error);
            }

            // Checks and parsing done! Send to sensorbehaviors
            foreach (INithSensorBehavior sbeh in SensorBehaviors)
            {
                sbeh.HandleData(data);
            }

            LastSensorData = data;
            LastError = error;
        }
    }
}