using System.Collections.Generic;
using CustomCapes.Common.Config;

namespace CustomCapes.Internal.Config {

    public class AppConfig : IConfigStruct {

        #region Fields

        #endregion

        #region Properties

        public string CapeServerUrl { get; set; } = "s.optifine.net";
        public List<User> Users { get; set; } = new List<User>();

        #endregion

        #region Constructor
 
        public AppConfig() {
            
        }

        #endregion

        #region Methods

        #endregion
        
    }

}