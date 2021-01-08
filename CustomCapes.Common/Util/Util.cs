using System;
using System.Diagnostics;
using System.IO;

namespace CustomCapes.Common.Util {

    public static class Util {

        public static void ExecuteCMD(string command) {
            var startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/C {command}"
            };
            new Process { StartInfo = startInfo }.Start();
        }

        public static string GetWindowsFolder() {
            return Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        }

        public static string GetHostsFileName() {
            return Path.Combine(GetWindowsFolder(), @"System32\drivers\etc\hosts");
        }
        
    }

}