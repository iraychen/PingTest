using PingTest.Bussiness;
using PingTest.Entity;
using PingTest.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingTest
{
    public class ConsoleReport:IExport
    {
        public void Export(NetworkData networkData)
        {
            Console.WriteLine();
            Console.WriteLine(MessageConstant.IPAddress, networkData.IPAddress);
            Console.WriteLine();

            Console.WriteLine(string.Format(MessageConstant.FromTimeToTime, networkData.FromTime, networkData.ToTime));

            if (networkData.TotalDuration.TotalMinutes > 1)
            {
                Console.WriteLine(string.Format(MessageConstant.TotalDuration, (int)networkData.TotalDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                Console.WriteLine(string.Format(MessageConstant.TotalDuration, (int)networkData.TotalDuration.TotalSeconds, MessageConstant.Second));
            }

            if (networkData.OfflineTotalDuration.TotalMinutes > 1)
            {
                Console.WriteLine(string.Format(MessageConstant.OfflineTotalDuration, (int)networkData.OfflineTotalDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                Console.WriteLine(string.Format(MessageConstant.OfflineTotalDuration, (int)networkData.OfflineTotalDuration.TotalSeconds, MessageConstant.Second));
            }


            if (networkData.OfflineLongestDuration.TotalMinutes > 1)
            {
                Console.WriteLine(string.Format(MessageConstant.OfflineLongestDuration, (int)networkData.OfflineLongestDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                Console.WriteLine(string.Format(MessageConstant.OfflineLongestDuration, (int)networkData.OfflineLongestDuration.TotalSeconds, MessageConstant.Second));
            }

            if (networkData.OnlineLongestDuration.TotalMinutes > 1)
            {
                Console.WriteLine(string.Format(MessageConstant.OnlineLongestDuration, (int)networkData.OnlineLongestDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                Console.WriteLine(string.Format(MessageConstant.OnlineLongestDuration, (int)networkData.OnlineLongestDuration.TotalSeconds, MessageConstant.Second));
            }



            Console.WriteLine(MessageConstant.OfflineCount, networkData.OfflineCount);
            
            Console.WriteLine();
        }
    }
}
