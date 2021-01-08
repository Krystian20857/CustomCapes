using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CustomCapes.Common.Util {

    public static class NetHelper {

        public static void AddMapping(IPAddress listenAddress, short listenPort, IPAddress connectAddress, short connectPort) {
            var command = $"netsh interface portproxy add v4tov4" +
                          $" listenport={listenPort} listenaddress={listenAddress}" +
                          $" connectport={connectPort} connectaddress={connectAddress}";
            Util.ExecuteCMD(command);
        }

        public static void RemoveMapping(IPAddress listenAddress, short listenPort) {
            var command = $"netsh interface portproxy delete v4tov4 " +
                          $"listenport={listenPort} listenaddress={listenAddress}";
            Util.ExecuteCMD(command);
        }

        public static void AppendHosts(IPAddress ipAddress, string host) {
            var hostsFile = Util.GetHostsFileName();
            var hostsLine = $"{ipAddress}\t{host}";
            using (var stream = File.AppendText(hostsFile)) {
                stream.Write(hostsLine);
            }
        }

        public static void RemoveFromHosts(IPAddress ipAddress, string host) {
            var hostsFile = Util.GetHostsFileName();
            var lines = File.ReadAllLines(hostsFile).ToList();
            var ipString = ipAddress.ToString();
            lines.RemoveAll(x => x.IndexOf(ipString, StringComparison.InvariantCultureIgnoreCase) >= 0 &&
                                 x.IndexOf(host, StringComparison.InvariantCultureIgnoreCase) >= 0);
            using (var stream = File.Open(hostsFile, FileMode.Truncate)) {
                foreach (var line in lines) {
                    var buffer = Encoding.UTF8.GetBytes(line);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.WriteByte((byte) '\n');
                }
            }
        }
        
    }

}