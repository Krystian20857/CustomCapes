using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using CustomCapes.Internal.Config;
using CustomCapes.Internal.Manager;
using Microsoft.Win32;

namespace CustomCapes.ViewModels {

    public class UserAddViewModel : Screen, INotifyPropertyChanged {

        #region Fields

        private UserManager UserManager => Bootstrapper.Instance.UserManager;

        #endregion

        #region Properties

        public string UserName { get; set; }
        public string CapePath { get; set; }

        public User CreatedUser { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void OnCancelClick() {
            TryCloseAsync();
        }

        public void OnAddClick() {
            if(UserManager.Exists(UserName) || !File.Exists(CapePath))
                return;
            CreatedUser = new User {
                Username = UserName,
                UUID = Guid.NewGuid()
            };
            UserManager.AddUser(CreatedUser);
            UserManager.CopyCapeFile(CreatedUser, CapePath);
            TryCloseAsync();
        }

        public void CapeBrowseClick() {
            var capePath = UserManager.ShowCapeDialog();
            if(string.IsNullOrEmpty(capePath))
                return;
            CapePath = string.Copy(capePath);
        }
        
        #endregion
        
    }

}