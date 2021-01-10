using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace CustomCapes {

    public class TrayIcon {

        #region Fields

        #endregion

        #region Properties

        public Icon Icon { get; }
        public NotifyIcon NotifyIcon { get; private set; }

        #endregion

        #region Constructor

        public TrayIcon() {
            
        }

        #endregion

        #region Methods

        public void Create() {
            NotifyIcon = new NotifyIcon();
            
            NotifyIcon.Text = "Custom Capes";
            NotifyIcon.Icon = Resources.Icon;

            NotifyIcon.Click += (sender, args) => Bootstrapper.Instance.SetForeground();
            
            NotifyIcon.ContextMenu = new ContextMenu();
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Open Custom Capes", (sender, args) => Bootstrapper.Instance.SetForeground()));
            NotifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", (sender, args) => Application.Current.Shutdown()));

            NotifyIcon.Visible = true;
        }

        #endregion
        
    }

}