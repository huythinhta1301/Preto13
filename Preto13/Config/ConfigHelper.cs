namespace Preto13.Config
{
    public class ConfigHelper
    {
        private static IConfigurationRoot configuration;

        static ConfigHelper()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string GetAppSetting(string key)
        {
            return configuration.GetSection("SecureStore")[key];
        }
    }
}
