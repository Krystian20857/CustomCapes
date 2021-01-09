using System;
using System.Net;
using System.Net.Http;
using CustomCapes.Common.Server;
using CustomCapes.Common.Util;
using CustomCapes.Internal.Config;
using NLog;

namespace CustomCapes.Internal.Manager {

    public class ServerManager {

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private readonly IHttpServer _server = new SimpleHttpServer();
        
        private readonly HttpClient _client = new HttpClient();

        #endregion

        #region Properties

        public IHttpServer Server => _server;
        private string CapesServer { get; }
        private IPAddress CapesServerIp { get; }

        #endregion

        #region Constructor

        public ServerManager() {
            
            recheckDNS:
            CapesServer = ConfigManager.Config.CapeServerUrl;
            var addresses = Dns.GetHostAddresses(CapesServer);
            if (addresses.Length == 0) {
                logger.Warn("Cannot find cape server.");
                return;
            }

            if (IPAddress.IsLoopback(addresses[0])) {
                NetHelper.RemoveFromHosts(IPAddress.Loopback, CapesServer);
                goto recheckDNS;
            }

            CapesServerIp = addresses[0];
            
            _server.HostFolder(Paths.CapesPath, "*.png");
            
            _server.ErrorEvent += (sender, exception) => {
                logger.Warn("Http serv error: ");
                logger.Warn(exception);
            };
             
            _server.FileNotFound += (sender, context) => {
                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.ContentType = "image/png";
                context.Response.AddHeader("Date", DateTime.Now.ToString("R"));
                context.Response.AddHeader("Last-Modified", DateTime.Now.ToString("R"));
                var stream = context.Response.OutputStream;
                var responseStream = _client.GetStreamAsync($"http://{CapesServerIp}:80{context.Request.Url.AbsolutePath}").Result;
                var count = 0;
                do {
                    var buffer = new byte[1024];
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, count);
                } while (responseStream.CanRead && count > 0);
                stream.Close();
                responseStream.Close();
            };
        }

        #endregion

        #region Methods

        public void StartServer() {
            _server.Initialize(IPAddress.Loopback, 80);
            logger.Info($"Started http server on port: {_server.Port}");
            NetHelper.AppendHosts(IPAddress.Loopback, CapesServer);
            logger.Info($"Overriden hosts file.");
        }

        public void StopServer() {
            _server.ForceStop();
            _server.ForceStop();
            logger.Info("Stopped http server.");
            NetHelper.RemoveFromHosts(IPAddress.Loopback, CapesServer);
            logger.Info("Removed records from hosts files.");
        }

        #endregion
        
    }

}