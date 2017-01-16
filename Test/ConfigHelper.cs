using System.Configuration;
using System.Xml;

namespace cnblogs.Jackson0714.Test
{
    public static class ConfigHelper
    {
        public static string ReadConfigurationByKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void SetConfigurationByKeyAndValue(string name,string value)
        {
            ConfigurationManager.AppSettings.Set(name, value);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[name].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static class AppKeys
        {
            public static string IP1 = "Target1";
            public static string IP2 = "Target2";
            public static string RetryInterval = "RetryInterval";
        }

        
    }
}
