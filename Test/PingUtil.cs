using System;
using System.Text;

namespace cnblogs.Jackson0714.Test
{
    public static class PingUtil
    {
        static System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
        static System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
        static string data = "Test Data Test Data Test Data!";
        static int timeout = 120;
        static System.Net.NetworkInformation.PingReply reply = null;
        static int retryInterval = int.Parse(ConfigHelper.ReadConfigurationByKey(ConfigHelper.AppKeys.RetryInterval));

        public static bool Ping(string ip)
        {
            options.DontFragment = true;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsConnected(string ipAddress)
        {
            bool isConnected = true;
            try
            {
                cnblogs.Jackson0714.RetryScope.RetryScope.BeginRetry(retryInterval);
                do
                {
                    try
                    {
                        try
                        {
                            if (!PingUtil.Ping(ipAddress))
                            {
                                throw new Exception(string.Format("{0} Disconnected", ipAddress), new Exception("Disconnected"));
                            }
                        }
                        finally
                        {

                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        if (!cnblogs.Jackson0714.RetryScope.RetryScope.HandleException(ex))
                        {
                            throw;
                        }
                    }
                } while (cnblogs.Jackson0714.RetryScope.RetryScope.Continue);

            }
            catch (Exception cex)
            {
                isConnected = false;
            }

            finally
            {
                cnblogs.Jackson0714.RetryScope.RetryScope.EndRetry(retryInterval);
            }
            return isConnected;
        }
    }
}
