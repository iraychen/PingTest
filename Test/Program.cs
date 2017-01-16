using log4net;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using cnblogs.Jackson0714.Framework.Logging;
using cnblogs.Jackson0714.PingTest.Entity;
using cnblogs.Jackson0714.Machine;
using cnblogs.Jackson0714.PingTest;
using cnblogs.Jackson0714.PingTest.Bussiness;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace cnblogs.Jackson0714.Test
{
    class Program
    {
        #region Private fields
        private static string logger = "RollingFileAppender";
        private static string target1VLANLogger = "Target1VLANLogger";
        private static string target2VLANLogger = "Target2VLANLogger";

        private static string target1AppenderName = "RollingFileAppenderTarget1";
        private static string target2AppenderName = "RollingFileAppenderTarget2";

        private static string log1FileName = "PingTest_Target1.txt";
        private static string log2FileName = "PingTest_Target2.txt";

        private static NetworkStatus targetIP1lastStatus = NetworkStatus.Unknown;
        private static NetworkStatus targetIP2lastStatus = NetworkStatus.Unknown;
        private static Logger log = null;
        private static Logger log1 = null;
        private static Logger log2 = null;
        private static string targetIP1 = string.Empty;
        private static string targetIP2 = string.Empty;
        private static string localMachineIP = string.Empty;
        private static int pingTarget1Count = -1;
        private static int pingTarget2Count = -1;
        private static int targetIP1GroupCount = 0;
        private static int targetIP2GroupCount = 0;
        private static int sleepTime = 1000;

        private static List<TimePoint> timePoints1 = new List<TimePoint>();
        private static List<TimePoint> timePoints2 = new List<TimePoint>();
        #endregion

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            // Prevent example from ending if CTL+C is pressed.
            Console.TreatControlCAsInput = true;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Press any combination of CTL, ALT, and SHIFT, and a console key.");
            Console.WriteLine("Press the Escape (Esc) key to quit.");
            Console.WriteLine("Press the Enter to export the report.");
            Console.ResetColor();

            #region Threads
            Thread testTargetIP1Connecting = new Thread(new ThreadStart(TestTargetIP1Connecting));
            Thread testTargetIP2Connecting = new Thread(new ThreadStart(TestTargetIP2Connecting));
            Thread testTargetIP1Disconnected = new Thread(new ThreadStart(TestTargetIP1Disconnected));
            Thread testTargetIP2Disconnected = new Thread(new ThreadStart(TestTargetIP2Disconnected));
            #endregion

            

            try
            {
                #region Init
                log = LoggerManager.GetLogger(logger);

                log1 = LoggerManager.GetLogger(target1VLANLogger);
                log2 = LoggerManager.GetLogger(target2VLANLogger);

                targetIP1 = ConfigHelper.ReadConfigurationByKey(ConfigHelper.AppKeys.IP1);
                targetIP2 = ConfigHelper.ReadConfigurationByKey(ConfigHelper.AppKeys.IP2);
                localMachineIP = MachineUtil.GetMachineIP();

                ChangeLogFileName();
                DeleteOriginalLogFile(log1FileName);
                DeleteOriginalLogFile(log2FileName);

                log.Info(string.Format("Started Testing."));
                Console.WriteLine("{0} Started Testing: From {1} To {2} ...", DateTime.Now, localMachineIP, targetIP1);
                log1.Info(string.Format("Started Testing: From {0} To {1} ...", localMachineIP, targetIP1));

                Console.WriteLine("{0} Started Testing: From {1} To {2} ...", DateTime.Now, localMachineIP, targetIP2);
                log2.Info(string.Format("Started Testing: From {0} To {1} ...", localMachineIP, targetIP2));

                #endregion

                #region Threads
                testTargetIP1Connecting.Start();
                testTargetIP2Connecting.Start();
                testTargetIP1Disconnected.Start();
                testTargetIP2Disconnected.Start();
                #endregion

                #region End

                

                do
                {
                    cki = Console.ReadKey();

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Press the Enter to export the report.");

                    Console.Write(" --- You pressed ");
                    if ((cki.Modifiers & ConsoleModifiers.Alt) != 0)
                    {
                        Console.Write("ALT+");
                    }
                    if ((cki.Modifiers & ConsoleModifiers.Shift) != 0)
                    {
                        Console.Write("SHIFT+");
                    }
                    if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                    {
                        Console.Write("CTL+");
                    }

                    Console.WriteLine(cki.Key.ToString());

                    if(cki.Key ==ConsoleKey.Enter)
                    {
                        List<TimePoint> currentTimePoints1 = timePoints1;
                        List<TimePoint> currentTimePoints2 = timePoints2;
                        currentTimePoints1.Add(new TimePoint(DateTime.Now, targetIP1lastStatus));
                        currentTimePoints2.Add(new TimePoint(DateTime.Now, targetIP2lastStatus));

                        #region Report
                        Console.ForegroundColor = ConsoleColor.Gray;

                        NetworkData networkData1 = NetworkSituationStatistic.Calculate(currentTimePoints1);
                        NetworkData networkData2 = NetworkSituationStatistic.Calculate(currentTimePoints2);
                        networkData1.IPAddress = targetIP1;
                        networkData2.IPAddress = targetIP2;
                        ConsoleReport consoleReport = new ConsoleReport();
                        consoleReport.Export(networkData1);
                        consoleReport.Export(networkData2);
                        LogFileReport logFileReport1 = new LogFileReport(log1);
                        LogFileReport logFileReport2 = new LogFileReport(log2);
                        logFileReport1.Export(networkData1);
                        logFileReport2.Export(networkData2);

                        #endregion

                    }
                    Console.ResetColor();

                    Thread.Sleep(500);

                }

                while (cki.Key != ConsoleKey.Escape);

                Console.TreatControlCAsInput = false;

                #endregion

                #region
            }
            catch (Exception ex)
            {
                log.Info("Error: \n" + ex.Message + "\r\n" + ex.StackTrace);
                Console.WriteLine("Error:\n" + ex.Message + "\r\n" + ex.StackTrace);
                Console.ReadKey();
            }
            finally
            {
                if (testTargetIP1Connecting != null)
                {
                    testTargetIP1Connecting.Abort();
                }
                if (testTargetIP2Connecting != null)
                {
                    testTargetIP1Connecting.Abort();
                }
                if (testTargetIP1Disconnected != null)
                {
                    testTargetIP1Connecting.Abort();
                }
                if (testTargetIP2Disconnected != null)
                {
                    testTargetIP1Connecting.Abort();
                }
                Console.WriteLine("{0} Testing Finished.", DateTime.Now);
                log1.Info("Testing Finished.");
                log2.Info("Testing Finished.");
            }
            #endregion

        }



        public static void ChangeLogFileName()
        {
            LoggerManager.ChangeFileName(target1AppenderName, "PingTest_From_" + localMachineIP + "_To_" + targetIP1 + ".txt");
            LoggerManager.ChangeFileName(target2AppenderName, "PingTest_From_" + localMachineIP + "_To_" + targetIP2 + ".txt");

        }
        public static void DeleteOriginalLogFile(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #region Format
        public static string FormatConnectingConsoleLog(string targetIPAddress, int pingTargetCount)
        {
            targetIPAddress = targetIPAddress.PadRight(15, ' ');
            return string.Format("{0} ----- {1} Connecting ------- Lost {2} package", DateTime.Now, targetIPAddress, pingTargetCount);
        }
        public static string FormatConnectingFileLog(string targetIPAddress, int pingTargetCount)
        {
            return string.Format("----- {0} Connecting ------- Lost {1} package\r\n", targetIPAddress, pingTargetCount);
        }
        public static string FormatDisconnectedConsoleLog(string targetIPAddress, int targetIPgroupCount)
        {
            targetIPAddress = targetIPAddress.PadRight(15, ' ');
            return string.Format("{0} ----- {1} Disconnected ----- {2}", DateTime.Now, targetIPAddress, targetIPgroupCount);
        }
        public static string FormatDisconnectedFileLog(string targetIPAddress, int targetIPgroupCount)
        {
            return string.Format("----- {0} Disconnected ----- {1}", targetIPAddress, targetIPgroupCount);
        }
        #endregion

        #region Using Threads To Ping IP Address.
        public static void TestTargetIP1Connecting()
        {
            while (true)
            {
                if (targetIP1lastStatus != NetworkStatus.Online)
                {
                    pingTarget1Count++;
                    if (PingUtil.IsConnected(targetIP1))
                    {
                        timePoints1.Add(new TimePoint(DateTime.Now, NetworkStatus.Online));
                        targetIP1lastStatus = NetworkStatus.Online;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(FormatConnectingConsoleLog(targetIP1, pingTarget1Count));
                        Console.ForegroundColor = ConsoleColor.White;
                        log1.Info(FormatConnectingFileLog(targetIP1, pingTarget1Count));
                    }
                }
                Thread.Sleep(sleepTime);
            }
        }

        public static void TestTargetIP2Connecting()
        {
            while (true)
            {
                if (targetIP2lastStatus != NetworkStatus.Online)
                {
                    pingTarget2Count++;
                    if (PingUtil.IsConnected(targetIP2))
                    {
                        timePoints2.Add(new TimePoint(DateTime.Now, NetworkStatus.Online));
                        targetIP2lastStatus = NetworkStatus.Online;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(FormatConnectingConsoleLog(targetIP2, pingTarget2Count));
                        Console.ForegroundColor = ConsoleColor.White;
                        log2.Info(FormatConnectingFileLog(targetIP2, pingTarget2Count));
                    }
                }
                Thread.Sleep(sleepTime);
            }
        }

        public static void TestTargetIP1Disconnected()
        {
            while (true)
            {
                if (targetIP1lastStatus != NetworkStatus.Offline && !PingUtil.IsConnected(targetIP1))
                {
                    timePoints1.Add(new TimePoint(DateTime.Now, NetworkStatus.Offline));

                    pingTarget1Count = 0;
                    targetIP1GroupCount++;

                    targetIP1lastStatus = NetworkStatus.Offline;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(FormatDisconnectedConsoleLog(targetIP1, targetIP1GroupCount));
                    Console.ForegroundColor = ConsoleColor.White;
                    log1.Info(FormatDisconnectedFileLog(targetIP1, targetIP1GroupCount));
                }
                Thread.Sleep(sleepTime);
            }
        }

        public static void TestTargetIP2Disconnected()
        {
            while (true)
            {
                if (targetIP2lastStatus != NetworkStatus.Offline && !PingUtil.IsConnected(targetIP2))
                {
                    timePoints2.Add(new TimePoint(DateTime.Now, NetworkStatus.Offline));

                    pingTarget2Count = 0;
                    targetIP2GroupCount++;

                    targetIP2lastStatus = NetworkStatus.Offline;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(FormatDisconnectedConsoleLog(targetIP2, targetIP2GroupCount));
                    Console.ForegroundColor = ConsoleColor.White;
                    log2.Info(FormatDisconnectedFileLog(targetIP2, targetIP2GroupCount));
                }
                Thread.Sleep(sleepTime);
            }
        }
        #endregion
    }
}




