using System;
using System.Net;

namespace CustomCapes.Common.Server {
    
    public interface IHttpServer {

        event EventHandler<HttpListenerContext> ReceivedContextEvent;
        event EventHandler<Exception> ErrorEvent;
        event EventHandler<HttpListenerContext> FileNotFound;

        bool IsRunning { get; }
        IPAddress Address { get; }
        short Port { get; }
        string ServerName { get; }

        void Initialize(IPAddress address, short port);
        bool HostFile(string uri, string path);
        bool HostFolder(string path, string searchPattern);
        void StopServer();
        void ForceStop();
        
    }
    
}