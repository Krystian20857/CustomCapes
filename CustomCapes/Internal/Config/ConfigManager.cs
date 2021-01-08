using CustomCapes.Common.Config;
using CustomCapes.Common.Config.Impl;

namespace CustomCapes.Internal.Config {

    public static class ConfigManager {

        #region Fields
        
        private static IConfiguration<AppConfig> _config = new YamlConfiguration<AppConfig>(Paths.ConfigFile);

        #endregion

        #region Properties

        public static AppConfig Config => _config.Config;
        public static IConfiguration<AppConfig> Manager => _config;

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public static void Save() {
            _config.Save();
        }

        #endregion

    }

}