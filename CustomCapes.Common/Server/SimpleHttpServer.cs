﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using CustomCapes.Common.Util;
using Ignite.SharpNetSH;

namespace CustomCapes.Common.Server {

    public class SimpleHttpServer : IHttpServer {

        #region Fields

        private Thread _listenerThread;
        private IPAddress _address;
        private short _port;
        private volatile bool _listen = true;
        private readonly Guid _serverUuid = Guid.NewGuid();
        
        private IDictionary<string, string> _hostedFiles = new ConcurrentDictionary<string, string>(2, 2);
        private IDictionary<string, string> _hostedFolders = new ConcurrentDictionary<string, string>(2, 2);

        public event EventHandler<HttpListenerContext> ReceivedContextEvent;
        public event EventHandler<Exception> ErrorEvent;

        #endregion

        #region Properties

        public bool IsRunning => _listen;
        public IPAddress Address => _address;
        public short Port => _port;

        public string ServerName => $"http://{_address}:{_port}/";

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void Initialize(IPAddress address, short port) {
            _address = address;
            _port = port;
            
            _listenerThread = new Thread(Listen) {
                IsBackground = true,
                Name = $"Simple Http Server: {_serverUuid}"
            };
            _listenerThread.SetApartmentState(ApartmentState.MTA);
            _listenerThread.Start();
        }

        public bool HostFile(string uri, string path) {
            if (!File.Exists(path) || _hostedFiles.ContainsKey(uri))
                return false;
            _hostedFiles.Add(uri, path);
            return true;
        }

        public bool HostFolder(string path, string searchPattern) {
            var dirName = Path.GetFileName(path);
            if (!Directory.Exists(path) || _hostedFolders.ContainsKey(dirName))
                return false;
            _hostedFolders.Add(dirName, path);
            return true;
        }


        public void StopServer() {
            _listen = false;
        }

        public void ForceStop() {
            StopServer();
            _listenerThread.Interrupt();
            _listenerThread.Abort();
        }

        private void Listen() {
            var listener = new HttpListener();
            listener.Prefixes.Add(ServerName);
            listener.IgnoreWriteExceptions = true;
            listener.Start();
            while (_listen) {
                try {
                    var context = listener.GetContext();
                    ProcessContext(context);
                    ReceivedContextEvent?.Invoke(this, context);
                } catch (Exception exception) {
                    ErrorEvent?.Invoke(this, exception);
                }
            }
            listener.Stop();
        }

        private void ProcessContext(HttpListenerContext context) {

            var mappings = GlobalTypeMappings.Mappings;
            
            var file = context.Request.Url.AbsolutePath.Substring(1);
            var filePath = _hostedFiles.TryGetValue(file, out var value) ? value : string.Empty;

            if (string.IsNullOrEmpty(filePath)) {
                foreach (var entry in _hostedFolders) {
                    var folderName = entry.Key;
                    var folderPath = entry.Value;
                    if (!file.StartsWith(folderName))
                        continue;
                    filePath = string.Copy(Path.Combine(Path.GetDirectoryName(folderPath), file));
                }
            }

            if (File.Exists(filePath)) {
                var stream = Stream.Null;
                try {
                    stream = new FileStream(filePath, FileMode.Open);

                    context.Response.ContentType = mappings.TryGetValue(Path.GetExtension(filePath), out var value0) ? value0 : "application/content";
                    context.Response.ContentLength64 = stream.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("R"));
                    context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filePath).ToString("r"));

                    var buffer = new byte[stream.Length];
                    var bytesToWrite = 0;

                    while ((bytesToWrite = stream.Read(buffer, 0, buffer.Length)) > 0) {
                        context.Response.OutputStream.Write(buffer, 0, bytesToWrite);
                    }

                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception exception) {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    ErrorEvent?.Invoke(this, exception);
                }
                finally {
                    stream.Close();
                }
            }
            else {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            
            context.Response.OutputStream.Close();

        }

        private Exception MakeFileNotFound(string file) {
            return new FileNotFoundException($"Cannot find file for http response: {file}");
        }

        public static void testRun() {
            IHttpServer httpServer = new SimpleHttpServer();
            httpServer.HostFolder(@"E:\capes", "*.*");
            httpServer.Initialize(IPAddress.Loopback, 80);
            NetHelper.AppendHosts(IPAddress.Loopback, "s.optifine.net");
            while (Console.ReadKey().Key != ConsoleKey.Escape) {
                
            }
            NetHelper.RemoveFromHosts(IPAddress.Loopback, "s.optifine.net");
            httpServer.StopServer();
        }

        #endregion
    }

}