using System;

namespace NITHdmis.NithSensors
{
    public abstract class ANithErrorStringBehavior : INithErrorBehavior
    {
        private NithModule nithModule;

        public ANithErrorStringBehavior(NithModule nithModule)
        {
            this.nithModule = nithModule;
        }

        public bool HandleError(NithErrors error)
        {
            string errStr = "";
            
                switch (error)
                {
                    case NithErrors.NaE:
                        return false;
                    case NithErrors.Connection:
                        errStr = "Error: no connection to any sensor on the selected port";
                        break;
                    case NithErrors.OutputCompliance:
                        errStr = "Error: the connected sensor was not recognized as a NITH sensor";
                        break;
                    case NithErrors.Name:
                        errStr = "Error: wrong sensor name or model connected. Compatible sensors are:\n";
                        foreach (NithSensorNames name in nithModule.ExpectedSensorNames)
                        {
                            errStr += name.ToString() + "\n";
                        }
                        break;
                    case NithErrors.Version:
                        errStr = "Error: wrong sensor version connected. Compatible versions are:\n";
                        foreach (string version in nithModule.ExpectedVersions)
                        {
                            errStr += version.ToString() + "\n";
                        }
                        break;
                    case NithErrors.StatusCode:
                        errStr = "Error: sensor sent an ERR status code (possible hardware error)";
                        break;
                    case NithErrors.Values:
                        errStr = "Error: the connected sensor does not provide the required arguments and values. Expected values are:\n";
                        foreach (NithArguments argument in nithModule.ExpectedArguments)
                        {
                            errStr += argument.ToString() + "\n";
                        }
                        break;
                    case NithErrors.OK:
                        errStr = "Sensor is operating normally!";
                        break;
                }

            ErrorActions(errStr, error);
            return true;
        }

        protected abstract void ErrorActions(string errorString, NithErrors error);
    }
}

