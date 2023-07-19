using System.Collections.Generic;
using System.IO.Ports;

namespace NITHdmis.ATmega
{
    public class SensorModule : SensorBase
    {
        public List<ISensorBehavior> Behaviors { get; set; }

        public SensorModule(int baudRate, string portPrefix = "COM") : base(baudRate, portPrefix)
        {
            Behaviors = new List<ISensorBehavior>();
        }

        protected override void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                foreach(ISensorBehavior behavior in Behaviors)
                {
                    behavior.ReceiveSensorRead(serialPort.ReadLine());
                    ReadError = false;
                }
            }
            catch
            {
                ReadError = true;
            }
        }
    }
}
