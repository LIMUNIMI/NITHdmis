using NITHdmis.ATmega;
using System.Collections.Generic;
using System.IO.Ports;

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

        public List<INithErrorBehavior> ErrorBehaviors { get; protected set; }
        public List<NithArguments> ExpectedArguments { get; set; } = new List<NithArguments>();
        public List<NithSensorNames> ExpectedSensorNames { get; set; } = new List<NithSensorNames>();
        public List<string> ExpectedVersions { get; set; } = new List<string>();
        public NithErrors LastError { get; protected set; }
        public NithSensorData LastSensorData { get; protected set; }
        public List<INithSensorBehavior> SensorBehaviors { get; protected set; }

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
                        error = NithErrors.OK; // Set to ok, then check if wrong
                        try
                        {
                            // Output splitting
                            string[] fields = line.Split('|');
                            string[] firstField = fields[0].Split('-');
                            string[] arguments = fields[2].Split('&');

                            // Parsings
                            data.RawLine = line;
                            data.SensorName = NithParsers.ParseSensorName(firstField[0].Trim(STARTSYMBOL));
                            data.Version = firstField[1];
                            data.StatusCode = NithParsers.ParseStatusCode(fields[1]);
                            foreach (string v in arguments)
                            {
                                string[] s = v.Split('=');
                                string argumentName = s[0];
                                NithArgumentValue value = new NithArgumentValue(NithParsers.ParseField(argumentName), s[1]);

                                data.Values.Add(value);
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
                                    if (ExpectedArguments.Count != 0 && !data.ContainsArguments(ExpectedArguments))
                                    {
                                        error = NithErrors.Values;
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