using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomCapes.Internal.Config;
using Microsoft.Win32;
using NLog;

namespace CustomCapes.Internal.Manager {

    public class UserManager {

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private AppConfig Config => ConfigManager.Config;

        #endregion

        #region Properties

        public List<User> Users => Config.Users;

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void AddUser(User user) {
            Users.Add(user);
            logger.Info("Added user: " + user.Username);
            ConfigManager.Save();
        }

        public void RemoveUser(User user) {
            Users.Remove(user);
            logger.Info("Removed user: " + user.Username);
            ConfigManager.Save();
        }

        public void RemoveUser(Guid uuid) {
            Users.RemoveAll(x => x.UUID == uuid);
            logger.Info("Removed users: " + uuid);
            ConfigManager.Save();
        }

        public User GetUserById(Guid uuid) {
            return Users.Find(x=> x.UUID == uuid);
        }

        public bool Exists(string userName) {
            return Users.Any(x => x.Username == userName);
        }
        
        public static void CopyCapeFile(User user, string file) {
            var destFile = MakeCapeFile(user);
            if(destFile == null)
                return;
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(file, destFile, true);
            logger.Info($"Copied cape png from: {file} to: {destFile}");
        }

        public static void RemoveCapeFile(User user) {
            var capeFile = MakeCapeFile(user);
            if (File.Exists(capeFile)) {
                File.Delete(capeFile);
                logger.Info("Removed cape png from: " + capeFile);
            }
        }

        public static string MakeCapeFile(User user) {
            return Path.Combine(Paths.CapesPath, user.Username + ".png");
        }

        public static string ShowCapeDialog() {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Cape files (*.png)|*.png";
            return fileDialog.ShowDialog() == true ? fileDialog.FileName : string.Empty;
        }
        
        #endregion
        
    }

}