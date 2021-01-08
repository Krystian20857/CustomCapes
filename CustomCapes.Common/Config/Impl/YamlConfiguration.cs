using System;
using System.IO;
using YamlDotNet.Serialization;

namespace CustomCapes.Common.Config.Impl {

    public class YamlConfiguration<TConfig> : IConfiguration<TConfig> where TConfig : IConfigStruct {

        #region Fields
        
        private readonly ISerializer _serializer = new SerializerBuilder()
            .IgnoreFields()
            .Build();
        
        private readonly IDeserializer _deserializer = new DeserializerBuilder()
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();

        #endregion

        #region Properties

        public string FilePath { get; }
        public TConfig Config { get; set; }

        #endregion

        #region Constructor

        public YamlConfiguration(string filePath) {
            FilePath = filePath;

            Load();
        }

        #endregion

        #region Methods

        public void Save() {
            if (!File.Exists(FilePath)) {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                Config = Activator.CreateInstance<TConfig>();
            }
            WriteAll(Config);
        }

        public void Load() {
            if(!File.Exists(FilePath))
                Save();
            else
                Config = ReadAll();
        }

        private void WriteAll(TConfig config) {
            File.WriteAllText(FilePath, _serializer.Serialize(config));
        }

        private TConfig ReadAll() {
            return _deserializer.Deserialize<TConfig>(File.ReadAllText(FilePath));
        }
        
        #endregion
    }

}