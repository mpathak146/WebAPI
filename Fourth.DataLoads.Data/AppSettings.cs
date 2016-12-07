using System;
using System.Configuration;
using System.Globalization;

namespace Fourth.DataLoads.Data
{
    public static class AppSettings
    {       

        public static string PathConfig
        {
            get
            {
                return Setting<string>("PathConfig");
            }
        }
        public static int BatchSize
        {
            get
            {
                int result;
                if (int.TryParse(Setting<string>("BatchSize"), out result))
                    return result;
                else
                    return 1000;
            }
        }
        public static string DataloadContext
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString;
            }
        }

        private static T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

    }
}
