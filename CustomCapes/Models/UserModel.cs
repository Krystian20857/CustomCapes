using System;
using System.ComponentModel;
using System.Windows.Media;
using Caliburn.Micro;
using CustomCapes.Common.MinecraftApi;
using CustomCapes.Common.Util;
using CustomCapes.Internal.Config;

namespace CustomCapes.Models {

    public class UserModel : INotifyPropertyChanged  {

        #region Fields

        private static MinecraftApiManager _mc = new MinecraftApiManager();
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion

        #region Properties

        public string UserName { get; set; }
        public Guid UUID { get; set; }
        public ImageSource Image { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public async void UpdateImage() {
            var response = await _mc.GetUUIDAsync(UserName);
            var uuidString = response == null ? Guid.Empty.ToString() : response.UUID;
            await _mc.GetHeadRenderAsync(uuidString)
                .ContinueWith(result => Execute.OnUIThread(() =>  Image = result.Result ));
        }


        public static UserModel FromUser(User user) {
            var model = new UserModel {
                UserName = user.Username,
                UUID = user.UUID
            };
            model.UpdateImage();
            return model;
        }

        public User ToUser() {
            return new User {
                Username = this.UserName,
                UUID = this.UUID
            };
        }
        
        #endregion

    }

}