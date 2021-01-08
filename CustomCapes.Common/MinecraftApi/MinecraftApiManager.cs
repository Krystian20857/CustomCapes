using System;
using System.Net.Http;

using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace CustomCapes.Common.MinecraftApi {

    public class MinecraftApiManager : IDisposable {

        #region Fields

        private readonly HttpClient _client = new HttpClient();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public async Task<UUIDResponse> GetUUIDAsync(string username) {
            var response = await _client.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
            
            if (response.IsSuccessStatusCode) {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UUIDResponse>(responseString);
            }

            return new UUIDResponse();
        }

        public async Task<ImageSource> GetHeadRenderAsync(string uuid){
            var response = await _client.GetAsync($"https://crafatar.com/renders/head/{uuid}?default=MHF_Steve&overlay");

            var image = new BitmapImage();
            if (response.IsSuccessStatusCode) {
                image.BeginInit();
                image.StreamSource = await response.Content.ReadAsStreamAsync();
                image.EndInit();
            }

            return image;
        }
        
        public void Dispose() {
            _client?.Dispose();
        }

        #endregion
    }

}