using System.IO.Ports;

namespace NITHdmis.ATmega
{
    public abstract class SensorBase
    {
        protected SerialPort serialPort;
        public bool ReadError { get; protected set; } = false;

        public bool IsConnectionOk { get; set; } = false;

        public string PortPrefix { get; set; }

        public SensorBase(int baudRate, string portPrefix = "COM")
        {
            PortPrefix = portPrefix;
            serialPort = new SerialPort();
            serialPort.BaudRate = baudRate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        public bool Connect(int portNumber)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            serialPort.PortName = PortPrefix + portNumber.ToString();

            try
            {
                serialPort.Open();
            }
            catch
            {
                IsConnectionOk = false;
                return false;
            }

            IsConnectionOk = true;
            return true;
        }

        public void Write(string str)
        {
            if (IsConnectionOk)
                serialPort.Write(str);
        }

        /// <summary>
        /// Closes the port. Returns true if the connection get closed. Returns false if the connection was already closed.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e);
    }
}
