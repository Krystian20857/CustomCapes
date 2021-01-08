using System.Net;
using CustomCapes.Common.Server;
using CustomCapes.Common.Util;
using CustomCapes.Internal.Config;
using NLog;

namespace CustomCapes.Internal.Manager {

    public class ServerManager {

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private readonly IHttpServer _server = new SimpleHttpServer();

        #endregion

        #region Properties

        public IHttpServer Server => _server;

        #endregion

        #region Constructor

        public ServerManager() {
            _server.HostFolder(Paths.CapesPath, "*.png");
            _server.ErrorEvent += (sender, exception) => {
                logger.Warn("Http serv error: ");
                logger.Warn(exception);
            };
        }

        #endregion

        #region Methods

        public void StartServer() {
            _server.Initialize(IPAddress.Loopback, 80);
            logger.Info($"Started http server on port: {_server.Port}");
            NetHelper.AppendHosts(IPAddress.Loopback, ConfigManager.Config.CapeServerUrl);
            logger.Info($"Overriden hosts file.");
        }

        public void StopServer() {
            _server.ForceStop();
            logger.Info("Stopped http server.");
            NetHelper.RemoveFromHosts(IPAddress.Loopback, ConfigManager.Config.CapeServerUrl);
            logger.Info("Removed records from hosts files.");
        }

        #endregion
        
    }

}