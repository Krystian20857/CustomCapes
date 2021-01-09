using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Caliburn.Micro;
using CustomCapes.Common.Server;
using CustomCapes.Internal;
using CustomCapes.Internal.Config;
using CustomCapes.Internal.Manager;
using CustomCapes.Models;

namespace CustomCapes.ViewModels {

    public class MainViewModel : Screen, INotifyPropertyChanged {

        #region Fields
        
        private IHttpServer _httpServer = new SimpleHttpServer();
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion

        #region Properties

        private UserManager UserManager => Bootstrapper.Instance.UserManager;
        public SolidColorBrush StatusColor { get; set; }
        public BindableCollection<UserModel> Users { get; } = new BindableCollection<UserModel>();

        public bool IsStartVisible { get; set; }
        public bool IsStopVisible { get; set; }

        #endregion

        #region Constructor

        public MainViewModel() {
            StatusColor = new SolidColorBrush(Colors.Red);

            LoadUsers();

            Bootstrapper.Instance.ServerManager.StartServer();

            Application.Current.Exit += (sender, args) => Bootstrapper.Instance.ServerManager.StopServer();
        }

        #endregion

        #region Methods

        private void LoadUsers() {
            foreach (var user in UserManager.Users) {
                Users.Add(UserModel.FromUser(user));
            }
        }

        private void UpdateVisibility() {
            IsStartVisible = !_httpServer.IsRunning;
            IsStopVisible = _httpServer.IsRunning;
        }

        public void OnUserAddClick() {
            var userViewModel = IoC.Get<UserAddViewModel>();
            IoC.Get<IWindowManager>().ShowDialogAsync(userViewModel);
            var createdUser = userViewModel.CreatedUser;
            if(createdUser != null)
                Users.Add(UserModel.FromUser(createdUser));
        }

        public void OnUserRemoveClick(object obj) {
            if(!(obj is UserModel userModel))
                return;
            UserManager.RemoveCapeFile(UserManager.GetUserById(userModel.UUID));
            Bootstrapper.Instance.UserManager.RemoveUser(userModel.UUID);
            Users.Remove(userModel);
        }
        
        public void OnUserEditClick(object obj) {
            if(!(obj is UserModel userModel))
                return;
            var dialogFile = UserManager.ShowCapeDialog();
            if(!string.IsNullOrEmpty(dialogFile))
                UserManager.CopyCapeFile(UserManager.GetUserById(userModel.UUID), dialogFile);
        }
        

        #endregion

    }

}