namespace Services.LocalizationService
{
    public interface ILocalizationService
    {
        public string GetString(string key, string tableName = "Default Localization Table", params object[] arguments);

        public string GetStringByIndex(string key, int index, string tableName = "Default Localization Table",
            params object[] arguments);
    }
}