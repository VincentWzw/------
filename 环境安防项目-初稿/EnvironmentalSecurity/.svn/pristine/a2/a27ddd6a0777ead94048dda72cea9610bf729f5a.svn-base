using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ES.Utility
{
    public class SerialPortOperation
    {
        public static List<string> GetPorts()
        {
            List<string> portsReturn = new List<string>();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port.Length < 7)
                {
                    portsReturn.Add(port);
                }
            }
            return portsReturn;
        }
    }
}
