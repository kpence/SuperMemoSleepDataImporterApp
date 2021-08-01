using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FitbitSuperMemoSleepDataImporterApp
{
    public class Options
    {
        public static string ClientId = ConfigurationManager.AppSettings["FitbitConsumerKey"];
        public static string ClientSecret = ConfigurationManager.AppSettings["FitbitConsumerSecret"];
        public static string[] AllScopes = new string[] { "activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight" };
        public static string Get(string key) {
            return ConfigurationManager.AppSettings[key];
        }
        public static void Set(string key, string value) {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)  
            {  
                Console.WriteLine("Error writing app settings");  
            }  
        }
    }
}
