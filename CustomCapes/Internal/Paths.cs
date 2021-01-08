using System;
using System.IO;

namespace CustomCapes.Internal {

    public class Paths {

        public static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string DataFolder = Path.Combine(AppDataFolder, "CustomCapes");
        public static readonly string LogsFolder = Path.Combine(DataFolder, "logs");
        public static readonly string ConfigFile = Path.Combine(DataFolder, "config.yml");
        public static readonly string CapesPath = Path.Combine(DataFolder, "capes");

    }

}