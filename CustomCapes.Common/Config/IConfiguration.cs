namespace CustomCapes.Common.Config {

    public interface IConfiguration<TConfig> where TConfig : IConfigStruct {

        TConfig Config { get; set; }

        void Save();
        void Load();

    }

}