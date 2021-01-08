using Newtonsoft.Json;

namespace CustomCapes.Common.MinecraftApi {

    public class UUIDResponse {

        #region Fields

        #endregion

        #region Properties

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("id")]
        public string UUID { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        #endregion
        
    }

}