using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Framework.Machine
{
    public class MachineUtil
    {
        public static string GetMachineIP(Collection<string> IPCollection)
        {
            string machineIP = string.Empty;
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    foreach (string ip in IPCollection)
                    {
                        if (ipAddress.ToString().Contains(ip))
                        {
                            machineIP = ipAddress.ToString();
                            return machineIP;
                        }
                    }
                }
            }
            return machineIP;
        }

        public static string GetMachineIP()
        {
            string machineIP = string.Empty;
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    machineIP = ipAddress.ToString();
                }
            }
            return machineIP;
        }
    }
}
