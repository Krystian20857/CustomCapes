using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Caliburn.Micro;
using CustomCapes.Common.Native;
using CustomCapes.Common.Util;
using CustomCapes.Internal;
using CustomCapes.Internal.Config;
using CustomCapes.Internal.Manager;
using CustomCapes.Util;
using CustomCapes.ViewModels;
using CustomCapes.Views;
using Hardcodet.Wpf.TaskbarNotification;
using NLog;
using LogManager = NLog.LogManager;

namespace CustomCapes {

    public class Bootstrapper : BootstrapperBase {

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly uint showMessageMessage = User32.RegisterWindowMessage("{B44FBB8D-2990-48B8-BCF3-E36CD304447A}");
        private static Bootstrapper _instance;

        private readonly Mutex _mutex = new Mutex(true, "{B44FBB8D-2990-48B8-BCF3-E36CD304447A}");
        private readonly SimpleContainer _container = new SimpleContainer();
        private readonly NativeWindowWrapper _nativeWindow = new NativeWindowWrapper();

        #endregion

        #region Properties

        public static Bootstrapper Instance => _instance;

        public UserManager UserManager { get; } = new UserManager();
        public ServerManager ServerManager { get; } = new ServerManager();
        public TrayIcon TrayIcon { get; } = new TrayIcon();

        #endregion

        #region Constructor

        public Bootstrapper() {
            _instance = this;
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoggerUtil.ConfigureLogger(Paths.LogsFolder);
            
            var isAlone = _mutex.WaitOne(TimeSpan.Zero, true);
            if (isAlone) {
                 Initialize();
                 _mutex.ReleaseMutex();
                 TrayIcon.Create();
                 logger.Info("Application started.");
            }
            else {
                User32.PostMessage((IntPtr)0xFFFF, showMessageMessage, IntPtr.Zero, IntPtr.Zero);
                Application.Shutdown();
                return;
            }
            
            Application.DispatcherUnhandledException += (sender, args) => logger.Error(args.Exception);
            _nativeWindow.MessageReceived += (sender, message) => {
                if(message.Msg != showMessageMessage)
                    return;
                SetForeground();
            };
        }
        
        #endregion

        #region Methods

        protected override void Configure() {

            _container.Singleton<IWindowManager, ExtendedWindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            
            _container.Singleton<MainViewModel>();
            _container.PerRequest<UserAddViewModel>(); 

        }

        protected override void BuildUp(object instance) {
            _container.BuildUp(instance);
        }

        protected override object GetInstance(Type service, string key) {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return _container.GetAllInstances(service);
        }

        protected override void OnExit(object sender, EventArgs e) {
            logger.Info("Exiting application.");
            TrayIcon.NotifyIcon.Dispose();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            var settings = new Dictionary<string, object>();
            settings.Add("showWindow", true);
            DisplayRootViewFor<MainViewModel>(settings);
        }

        public void SetForeground()
        {
            var mainWindow = IoC.Get<MainViewModel>();
            if (mainWindow == null)
                return;
            var windowObject = mainWindow.GetView() as MainView;
            var hwnd = IntPtr.Zero;
            if(windowObject != null)
                hwnd = new WindowInteropHelper(windowObject).Handle;
            if (mainWindow.IsActive && hwnd != IntPtr.Zero) {
                windowObject.WindowState = WindowState.Normal;
            }
            if(hwnd == IntPtr.Zero)
                IoC.Get<IWindowManager>().ShowWindowAsync(mainWindow);

            User32.BringWindowToTop(hwnd);
            User32.SetForegroundWindow(hwnd);
        }

        #endregion
        
    }

}