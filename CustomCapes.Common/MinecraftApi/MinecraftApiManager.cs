using System;
using System.Collections.Generic;
using System.Net;
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

        public MinecraftApiManager() {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        
        #endregion

        #region Methods

        public async Task<UUIDResponse> GetUUIDAsync(string username) {
            try
            {
                var response = await _client.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UUIDResponse>(responseString);
                }

                return new UUIDResponse();
            } catch (Exception exception) {
                return new UUIDResponse()
                {
                    Name = username,
                    UUID = Guid.Empty.ToString()
                };
            }
        }

        public async Task<ImageSource> GetHeadRenderAsync(string uuid){
            try
            {
                var response =
                    await _client.GetAsync($"https://crafatar.com/renders/head/{uuid}?default=MHF_Steve&overlay");

                var image = new BitmapImage();
                if (response.IsSuccessStatusCode)
                {
                    image.BeginInit();
                    image.StreamSource = await response.Content.ReadAsStreamAsync();
                    image.EndInit();
                }

                return image;
            } catch (Exception exception) {
                return null;
            }
        }
        
        public void Dispose() {
            _client?.Dispose();
        }

        #endregion
    }

}