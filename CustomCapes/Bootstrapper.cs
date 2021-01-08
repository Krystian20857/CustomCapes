using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using CustomCapes.Common.Util;
using CustomCapes.Internal;
using CustomCapes.Internal.Manager;
using CustomCapes.ViewModels;
using NLog;
using LogManager = NLog.LogManager;

namespace CustomCapes {

    public class Bootstrapper : BootstrapperBase {

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static Bootstrapper _instance;

        private readonly Mutex _mutex = new Mutex(true, "{B44FBB8D-2990-48B8-BCF3-E36CD304447A}");
        private readonly SimpleContainer _container = new SimpleContainer();

        #endregion

        #region Properties

        public static Bootstrapper Instance => _instance;

        public UserManager UserManager { get; } = new UserManager();
        public ServerManager ServerManager { get; } = new ServerManager();

        #endregion

        #region Constructor

        public Bootstrapper() {
            _instance = this;
            LoggerUtil.ConfigureLogger(Paths.LogsFolder);
            var isAlone = _mutex.WaitOne(TimeSpan.Zero, true);
            if (isAlone) {
                 Initialize();
                 logger.Info("Application started.");
            }
            else {
                MessageBox.Show("Application is currently running!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Shutdown();
            }
            Application.DispatcherUnhandledException += (sender, args) => logger.Error(args.Exception); 
        }
        
        #endregion

        #region Methods

        protected override void Configure() {

            _container.Singleton<IWindowManager, WindowManager>();
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
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewFor<MainViewModel>();
        }

        #endregion
        
    }

}