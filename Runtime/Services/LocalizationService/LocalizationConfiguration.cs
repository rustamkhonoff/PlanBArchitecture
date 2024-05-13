namespace LocalizationService
{
    public class LocalizationConfiguration
    {
        public string DefaultTableName { get; set; } = "Default Localization Table";
        public static LocalizationConfiguration Default => new LocalizationConfiguration();
    }
}