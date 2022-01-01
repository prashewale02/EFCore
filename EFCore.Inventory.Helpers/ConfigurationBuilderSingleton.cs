using Microsoft.Extensions.Configuration;

namespace EFCore.Inventory.Helpers
{
    public sealed class ConfigurationBuilderSingleton
    {

        #region Private Members 

        private static ConfigurationBuilderSingleton? _instance = null;
        private static readonly object instanceLock = new object();

        private static IConfigurationRoot? _configuration;

        #endregion

        #region Constructors

        private ConfigurationBuilderSingleton()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", 
                                    optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        #endregion

        #region Public Properties

        public static ConfigurationBuilderSingleton Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationBuilderSingleton();
                    }
                    return _instance;
                }
            }
        }

        public static IConfigurationRoot? ConfigurationRoot
        {
            get
            {
                if (_configuration == null) { var _ = Instance; }
                return _configuration;
            }
        }
        #endregion
    }
}
