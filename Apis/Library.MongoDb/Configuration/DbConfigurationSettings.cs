namespace Library.MongoDb.Configuration
{
    public class DbConfigurationSettings
    {
        public const string Section = "MongoSettings";

        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
